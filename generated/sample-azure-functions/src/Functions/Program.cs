using Application.Handlers;
using Application.Validators;
using FluentValidation;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Retry;

var host = new HostBuilder()
	.ConfigureFunctionsWorkerDefaults()
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();

		services.AddValidatorsFromAssemblyContaining<DocumentMetadataRequestValidator>();
		services.AddTransient<GetDocumentMetadataHandler>();
		services.AddTransient<GetDocumentFileHandler>();

		services.AddSingleton<ResiliencePipeline>(_ =>
		{
			var pipelineBuilder = new ResiliencePipelineBuilder();
			pipelineBuilder.AddRetry(new RetryStrategyOptions
			{
				MaxRetryAttempts = 2,
				Delay = TimeSpan.FromMilliseconds(200)
			});

			return pipelineBuilder.Build();
		});

		services.AddSingleton<Application.Interfaces.IDocumentService, InMemoryDocumentService>();
	})
	.Build();

host.Run();
