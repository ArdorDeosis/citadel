using Fyremoss.DependencyInjection;
using GameKernel;
using GameKernel.TickSystem;
using Model;
using Operations;
using State;

Thread.CurrentThread.Name = "Main Thread";

var configuration = new InjectorConfiguration();

configuration.Bind<ErrorHandler>().ToInstance(exception => Console.Error.WriteLine(exception.Message));

configuration.SetupTickSystem();

configuration.Bind<Repository>().AsSingleton();
configuration.Bind<OperationEngine>().AsSingleton();

var injector = configuration.BuildInjector();

var tickMaster = injector.Resolve<ITickMaster>();
var engine = injector.Resolve<OperationEngine>();
var repository = injector.Resolve<Repository>();

tickMaster.StartTicking();

engine.Execute(new CreateBuildingOperation(new BuildingType
{
  Id = Guid.CreateVersion7(),
  Name = "Super Building",
  WorkplaceCount = 1337,
  Recipes = [],
}));

tickMaster.WaitForFullTick();

Console.WriteLine(repository.Buildings.Values.First().Type.Name);
tickMaster.Stop();