using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Dispatching;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanTemplate.CommandHandler.Tests
{
    public class DispatcherTests
    {
        [Fact]
        public async Task Send_Should_DispatchRequestToMatchingHandler()
        {
            var serviceProvider = new ServiceCollection()
                .AddDispatching(typeof(ScannedRequestHandler).Assembly)
                .BuildServiceProvider();
            var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

            var result = await dispatcher.Send(new ScannedRequest("ping"));

            Assert.Equal("handled: ping", result);
        }

        [Fact]
        public async Task Send_Should_PassCancellationTokenToHandler()
        {
            var recorder = new DispatcherRecorder();
            var cancellationTokenSource = new CancellationTokenSource();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(recorder)
                .AddTransient<IRequestHandler<TokenRequest, string>, TokenRequestHandler>()
                .AddTransient<IDispatcher, Dispatcher>()
                .BuildServiceProvider();
            var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

            await dispatcher.Send(new TokenRequest(), cancellationTokenSource.Token);

            Assert.Equal(cancellationTokenSource.Token, recorder.CancellationToken);
        }

        [Fact]
        public async Task Send_Should_ExecutePipelineBehaviorsInRegistrationOrder()
        {
            var recorder = new DispatcherRecorder();
            var serviceProvider = new ServiceCollection()
                .AddSingleton(recorder)
                .AddTransient<IRequestHandler<PipelineRequest, string>, PipelineRequestHandler>()
                .AddTransient<IRequestPipelineBehavior<PipelineRequest, string>, FirstPipelineBehavior>()
                .AddTransient<IRequestPipelineBehavior<PipelineRequest, string>, SecondPipelineBehavior>()
                .AddTransient<IDispatcher, Dispatcher>()
                .BuildServiceProvider();
            var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

            var result = await dispatcher.Send(new PipelineRequest());

            Assert.Equal("pipeline-result", result);
            Assert.Equal(
                new[] { "first-before", "second-before", "handler", "second-after", "first-after" },
                recorder.Steps);
        }

        [Fact]
        public async Task Send_Should_Throw_WhenHandlerIsMissing()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<IDispatcher, Dispatcher>()
                .BuildServiceProvider();
            var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => dispatcher.Send(new MissingHandlerRequest()));
        }

        [Fact]
        public async Task Send_Should_Throw_WhenMultipleHandlersAreRegistered()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<IRequestHandler<DuplicateHandlerRequest, string>, DuplicateHandlerRequestHandler>()
                .AddTransient<IRequestHandler<DuplicateHandlerRequest, string>, OtherDuplicateHandlerRequestHandler>()
                .AddTransient<IDispatcher, Dispatcher>()
                .BuildServiceProvider();
            var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => dispatcher.Send(new DuplicateHandlerRequest()));
        }

        private sealed class DispatcherRecorder
        {
            public List<string> Steps { get; } = new List<string>();

            public CancellationToken CancellationToken { get; set; }
        }

        private sealed class ScannedRequest : IRequest<string>
        {
            public ScannedRequest(string value)
            {
                Value = value;
            }

            public string Value { get; }
        }

        private sealed class ScannedRequestHandler : IRequestHandler<ScannedRequest, string>
        {
            public Task<string> Handle(ScannedRequest request, CancellationToken cancellationToken)
            {
                return Task.FromResult($"handled: {request.Value}");
            }
        }

        private sealed class TokenRequest : IRequest<string>
        {
        }

        private sealed class TokenRequestHandler : IRequestHandler<TokenRequest, string>
        {
            private readonly DispatcherRecorder _recorder;

            public TokenRequestHandler(DispatcherRecorder recorder)
            {
                _recorder = recorder;
            }

            public Task<string> Handle(TokenRequest request, CancellationToken cancellationToken)
            {
                _recorder.CancellationToken = cancellationToken;
                return Task.FromResult("token-result");
            }
        }

        private sealed class PipelineRequest : IRequest<string>
        {
        }

        private sealed class PipelineRequestHandler : IRequestHandler<PipelineRequest, string>
        {
            private readonly DispatcherRecorder _recorder;

            public PipelineRequestHandler(DispatcherRecorder recorder)
            {
                _recorder = recorder;
            }

            public Task<string> Handle(PipelineRequest request, CancellationToken cancellationToken)
            {
                _recorder.Steps.Add("handler");
                return Task.FromResult("pipeline-result");
            }
        }

        private sealed class FirstPipelineBehavior : IRequestPipelineBehavior<PipelineRequest, string>
        {
            private readonly DispatcherRecorder _recorder;

            public FirstPipelineBehavior(DispatcherRecorder recorder)
            {
                _recorder = recorder;
            }

            public async Task<string> Handle(PipelineRequest request, RequestHandlerDelegate<string> next, CancellationToken cancellationToken)
            {
                _recorder.Steps.Add("first-before");
                var result = await next();
                _recorder.Steps.Add("first-after");
                return result;
            }
        }

        private sealed class SecondPipelineBehavior : IRequestPipelineBehavior<PipelineRequest, string>
        {
            private readonly DispatcherRecorder _recorder;

            public SecondPipelineBehavior(DispatcherRecorder recorder)
            {
                _recorder = recorder;
            }

            public async Task<string> Handle(PipelineRequest request, RequestHandlerDelegate<string> next, CancellationToken cancellationToken)
            {
                _recorder.Steps.Add("second-before");
                var result = await next();
                _recorder.Steps.Add("second-after");
                return result;
            }
        }

        private sealed class MissingHandlerRequest : IRequest<string>
        {
        }

        private sealed class DuplicateHandlerRequest : IRequest<string>
        {
        }

        private sealed class DuplicateHandlerRequestHandler : IRequestHandler<DuplicateHandlerRequest, string>
        {
            public Task<string> Handle(DuplicateHandlerRequest request, CancellationToken cancellationToken)
            {
                return Task.FromResult("first");
            }
        }

        private sealed class OtherDuplicateHandlerRequestHandler : IRequestHandler<DuplicateHandlerRequest, string>
        {
            public Task<string> Handle(DuplicateHandlerRequest request, CancellationToken cancellationToken)
            {
                return Task.FromResult("second");
            }
        }
    }
}
