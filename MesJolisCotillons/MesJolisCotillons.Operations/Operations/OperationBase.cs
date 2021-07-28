////namespace MesJolisCotillons.Operations.User.Create
////{
////    using System.Threading.Tasks;
////    using MesJolisCotillons.Adapters.User.Create;
////    using MesJolisCotillons.Commands.Builders.User.Create;
////    using MesJolisCotillons.Commands.User.Create;
////    using MesJolisCotillons.Contracts.Requests.User.Create;
////    using MesJolisCotillons.Contracts.Responses.User.Create;
////    using MesJolisCotillons.Core;
////    using MesJolisCotillons.Core.Framework.Builders.Operation;
////    using MesJolisCotillons.Executors.Builders.User.Create;
////    using MesJolisCotillons.Response.Builders.User.Create;
////    using MesJolisCotillons.Validation.Builders.CreateUser;

////    public abstract class OperationBase<TRequest, TResponse>
////    {
////        private readonly IOperationBuilder<CreateUserRequest, CreateUserCommand, ICreateUserAdapter> operationBuilder;
////        private readonly ICreateUserAdapter adapter;
////        private readonly ICreateUserCommandBuilder commandBuilder;
////        private readonly ICreateUserValidationBuilder validationBuilder;
////        private readonly ICreateUserExecutorBuilder executorBuilder;
////        private readonly ICreateUserResponseBuilder responseBuilder;

////        public CreateUserOperationDeux(
////            IOperationBuilder<CreateUserRequest, CreateUserCommand, ICreateUserAdapter> operationBuilder,
////            ICreateUserAdapter adapter,
////            ICreateUserCommandBuilder commandBuilder,
////            ICreateUserValidationBuilder validationBuilder,
////            ICreateUserExecutorBuilder executorBuilder,
////            ICreateUserResponseBuilder responseBuilder)
////        {
////            this.operationBuilder = operationBuilder;
////            this.adapter = adapter;
////            this.commandBuilder = commandBuilder;
////            this.validationBuilder = validationBuilder;
////            this.executorBuilder = executorBuilder;
////            this.responseBuilder = responseBuilder;
////        }

////        public Task<CreateUserResponse> Run(CreateUserRequest request)
////        {
////            return this.operationBuilder
////                    .AddAdapter(this.adapter, request)
////                    .AddCommandBuilder(this.commandBuilder)
////                    .AddValidationBuilder(this.validationBuilder)
////                    .AddExecutorBuilder(this.executorBuilder)
////                    .AddResponseBuilder(this.responseBuilder)
////                    .Build()
////                    .Run();
////        }

////        public Task<TResponse> Run(TRequest request)
////        {
////            throw new System.NotImplementedException();
////        }
////    }
////}
