using FluentAssertions;
using MesJolisCotillons.Common.UnitTests.Fake;
using MesJolisCotillons.Core.Framework.Builders.Executor;
using MesJolisCotillons.Core.Framework.Builders.Operation;
using MesJolisCotillons.Core.Framework.Builders.Validation;
using MesJolisCotillons.DataAccess.Entities.Context;
using MesJolisCotillons.Executors;
using MesJolisCotillons.Executors.Builder;
using MesJolisCotillons.Response.Builders;
using MesJolisCotillons.Response.Builders.Failure;
using MesJolisCotillons.Validation.Builders;
using MesJolisCotillons.Validation.Validators;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Core.UnitTests.Framework.Operation
{
    public class ProcessOperationFactoryTests
    {
        public abstract class ProcessOperationFactoryTest
        {
            #region Mocks properties
            protected FakeCommand CommandMock { get; set; }

            protected IValidationReport<FakeCommand> ValidationReportMock { get; set; }

            protected IDbContextTransaction TransactionMock { get; set; }

            protected ISaveDbContext SaveDbContextMock { get; set; }

            protected IValidatorsProcessor<FakeCommand> ValidatorsProcessorMock { get; set; }

            protected IExecutorsProcessor<FakeCommand> ExecutorsProcessorMock { get; set; }

            protected IFailureResponseBuilder<FakeCommand, FakeResponse> FailureReponseBuilderMock { get; set; }

            protected Func<Task<FakeCommand>> CommandGeneratorMock { get; set; }

            protected IValidationBuilder<FakeCommand> ValidationBuilderMock { get; set; }

            protected IExecutorBuilder<FakeCommand> ExecutorBuilderMock { get; set; }

            protected IResponseBuilder<FakeCommand, FakeResponse> ResponseBuilderMock { get; set; }
            #endregion

            protected ProcessOperationFactory<FakeCommand, FakeResponse> ProcessOperationFactory { get; set; }

            public ProcessOperationFactoryTest()
            {
                this.CommandMock = new FakeCommand();
                this.SaveDbContextMock = Substitute.For<ISaveDbContext>();
                this.TransactionMock = Substitute.For<IDbContextTransaction>();
                this.ValidatorsProcessorMock = Substitute.For<IValidatorsProcessor<FakeCommand>>();
                this.ExecutorsProcessorMock = Substitute.For<IExecutorsProcessor<FakeCommand>>();
                this.FailureReponseBuilderMock = Substitute.For<IFailureResponseBuilder<FakeCommand, FakeResponse>>();

                this.CommandGeneratorMock = () => Task.FromResult(this.CommandMock);
                this.ValidationBuilderMock = Substitute.For<IValidationBuilder<FakeCommand>>();
                this.ExecutorBuilderMock = Substitute.For<IExecutorBuilder<FakeCommand>>();
                this.ResponseBuilderMock = Substitute.For<IResponseBuilder<FakeCommand, FakeResponse>>();

                this.FailureReponseBuilderMock.Build(Arg.Any<FakeResponse>(), this.ValidationReportMock)
                    .Returns(new FakeResponse(false));

                this.ProcessOperationFactory = new ProcessOperationFactory<FakeCommand, FakeResponse>(
                    this.SaveDbContextMock,
                    this.ValidatorsProcessorMock,
                    this.ExecutorsProcessorMock,
                    this.FailureReponseBuilderMock);
            }
        }

        public class CreateProcessOperationShould : ProcessOperationFactoryTest
        {
            [Fact]
            public async Task Return_Expected_ResponseType()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                responseResult.Should().BeOfType<FakeResponse>();
            }

            [Fact]
            public async Task Return_NotNull_Response()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                responseResult.Should().NotBeNull();
            }

            [Fact]
            public async Task Not_Call_ValidationLogic_When_ValidationBuilder_IsNull()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    null,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.ValidationBuilderMock
                    .Received(0)
                    .Build();

                this.ValidatorsProcessorMock
                   .Received(0)
                   .ProcessValidators(Arg.Any<IEnumerable<IValidatorStep<FakeCommand>>>(), this.CommandMock);
            }

            [Fact]
            public async Task Call_ValidationLogic_When_ValidationBuilder_IsNotNull()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.ValidationBuilderMock
                    .Received(1)
                    .Build();

                this.ValidatorsProcessorMock
                   .Received(1)
                   .ProcessValidators(Arg.Any<IEnumerable<IValidatorStep<FakeCommand>>>(), this.CommandMock);
            }

            [Fact]
            public async Task Call_ExecutorLogic_When_ValidationBuilder_IsNull()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    null,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.ExecutorBuilderMock
                    .Received(1)
                    .Build(this.CommandMock);

                await this.ExecutorsProcessorMock
                    .Received(1)
                    .ProcessExecutors(Arg.Any<IEnumerable<IExecutorStep<FakeCommand>>>(), this.CommandMock);
            }

            [Fact]
            public async Task Not_Call_ExecutorLogic_When_ExecutorBuilder_IsNull()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    null,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.ExecutorBuilderMock
                    .Received(0)
                    .Build(this.CommandMock);

                await this.ExecutorsProcessorMock
                    .Received(0)
                    .ProcessExecutors(Arg.Any<IEnumerable<IExecutorStep<FakeCommand>>>(), this.CommandMock);
            }

            [Fact]
            public async Task Call_ExecutorLogic_When_Validation_IsValid()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.ExecutorBuilderMock
                    .Received(1)
                    .Build(this.CommandMock);

                await this.ExecutorsProcessorMock
                    .Received(1)
                    .ProcessExecutors(Arg.Any<IEnumerable<IExecutorStep<FakeCommand>>>(), this.CommandMock);
            }

            [Fact]
            public async Task Call_FailureResponseBuilder_When_Validation_IsNotValid()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: false);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.FailureReponseBuilderMock
                    .Received(1)
                    .Build(Arg.Any<FakeResponse>(), this.ValidationReportMock);
            }

            [Fact]
            public async Task Not_Call_FailureResponseBuilder_When_Validation_IsValid()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.FailureReponseBuilderMock
                    .Received(0)
                    .Build(Arg.Any<FakeResponse>(), this.ValidationReportMock);
            }

            [Fact]
            public async Task Not_Call_ResponseBuilder_When_Validation_IsNotValid()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: false);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.ResponseBuilderMock
                    .Received(0)
                    .Build(Arg.Any<FakeResponse>(), this.ValidationReportMock);
            }

            [Fact]
            public async Task Call_ResponseBuilder_When_Validation_IsValid()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.ResponseBuilderMock
                    .Received(1)
                    .Build(Arg.Any<FakeResponse>(), this.ValidationReportMock);
            }

            [Fact]
            public async Task Call_BeginTransation_From_SaveDbContext()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.SaveDbContextMock
                    .Received(1)
                    .BeginTransaction();
            }

            [Fact]
            public async Task Call_Transaction_Commit_When_ProcessOperation_FinishedCorrectly()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                this.SaveDbContextMock
                 .BeginTransaction()
                 .Returns(this.TransactionMock);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                this.TransactionMock
                    .Received(1)
                    .Commit();

                this.TransactionMock
                    .Received(0)
                    .Rollback();
            }

            [Fact]
            public async Task Call_Transaction_Rollback_When_ProcessOperation_Raised_Exception()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                this.SaveDbContextMock
                 .BeginTransaction()
                 .Returns(this.TransactionMock);

                // Lets s Raise an exception when validationBuilder is called
                this.ValidationBuilderMock
                    .When(vb => vb.Build())
                    .Do(x => { throw new Exception(); });

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                try
                {
                    var responseResult = await processOperationFn();
                }
                catch (Exception)
                {
                    // Assert
                    this.TransactionMock
                           .Received(0)
                           .Commit();

                    this.TransactionMock
                        .Received(1)
                        .Rollback();
                }
            }

            [Fact]
            public async Task Call_Transaction_Dispose_When_ProcessOperation_Raised_Exception()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();
                this.ArrangeValidationProcessor(isValidationValid: true);

                this.SaveDbContextMock
                 .BeginTransaction()
                 .Returns(this.TransactionMock);

                // Lets s Raise an exception when validationBuilder is called
                this.ValidationBuilderMock
                    .When(vb => vb.Build())
                    .Do(x => { throw new Exception(); });

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                try
                {
                    var responseResult = await processOperationFn();
                }
                catch (Exception)
                {
                    // Assert
                    // Note: Cannot check Reiceived count = 1, because Dispose seems to be called twice
                    // So just a simple check if it is called.
                    this.TransactionMock
                        .Received()
                        .Dispose();
                }
            }

            [Fact]
            public async Task Call_Transaction_Dispose_When_ProcessOperation_FinishedCorrectly()
            {
                // Arrange
                this.ArrangeAnyOutputForParams();

                this.SaveDbContextMock
                 .BeginTransaction()
                 .Returns(this.TransactionMock);

                // arrange validation processors to send validation report with positive response
                this.ArrangeValidationProcessor(isValidationValid: true);

                // Act
                var processOperationFn = this.ProcessOperationFactory.CreateProcessOperation(
                    this.CommandGeneratorMock,
                    this.ValidationBuilderMock,
                    this.ExecutorBuilderMock,
                    this.ResponseBuilderMock);

                var responseResult = await processOperationFn();

                // Assert
                // Note: Cannot check Reiceived count = 1, because Dispose seems to be called twice
                // So just a simple check if it is called.
                this.TransactionMock
                    .Received()
                    .Dispose();
            }

            private void ArrangeValidationProcessor(bool isValidationValid = false)
            {
                this.ValidationReportMock = new ValidationReport<FakeCommand>(this.CommandMock, isValidationValid, new List<ValidatedCommand<FakeCommand>>());

                this.ValidatorsProcessorMock.ProcessValidators(Arg.Any<IEnumerable<IValidatorStep<FakeCommand>>>(), this.CommandMock)
                    .Returns(this.ValidationReportMock);
            }

            private void ArrangeAnyOutputForParams()
            {
                this.ValidationBuilderMock.Build()
                    .Returns(new List<IValidatorStep<FakeCommand>>());

                this.ExecutorBuilderMock.Build(Arg.Any<FakeCommand>())
                    .Returns(new List<IExecutorStep<FakeCommand>>());

                this.ResponseBuilderMock.Build(Arg.Any<FakeResponse>(), Arg.Any<IValidationReport<FakeCommand>>())
                    .Returns(new FakeResponse(true));
            }
        }
    }
}
