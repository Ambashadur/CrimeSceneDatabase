﻿using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

internal class Program
{
    private const string ENVIRONMENT_VARIABLE_NAME = "ASPNETCORE_ENVIRONMENT";
    private const string DATABASE_NAME = "Csd";

    private static void Main(string[] args) {
        using var serviceProvider = CreateServices();
        using var scope = serviceProvider.CreateScope();

        UpdateDatabase(scope.ServiceProvider);
    }

    private static ServiceProvider CreateServices() {
        var env = Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_NAME);
        var config = new ConfigurationBuilder()
            .AddJsonFile($"Configs/connectionStrings.{env}.json", true)
            .AddEnvironmentVariables()
            .Build();

        return new ServiceCollection()
            .AddSingleton<IConfiguration>(config)
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres10_0()
                .WithGlobalConnectionString(x => x.GetRequiredService<IConfiguration>().GetConnectionString(DATABASE_NAME))
                .WithGlobalCommandTimeout(TimeSpan.FromMinutes(5))
                .ScanIn(typeof(Program).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider) {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}