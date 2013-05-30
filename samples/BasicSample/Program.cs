using System;
using System.Text;
using Needle.Container;
using BasicSample.Classes;
using System.Threading.Tasks;

namespace BasicSample
{
	class Program
	{
		private static bool IsInterfaceMapped = false;
		private static bool IsWithoutDependenciesStored = false;
		private static WithoutDependencies LastWithoutDependencies = null;
		private static INeedleContainer Container = new NeedleContainer();

		static void Main(string[] args)
		{
			ConsoleKey option = ConsoleKey.D0;

			do
			{
				PrintMenu();
				var info = Console.ReadKey();
				option = info.Key;
				
				Console.WriteLine();
				Console.WriteLine();

				PerformAction(option);

				Console.WriteLine();
				Console.WriteLine();

			} while (option != ConsoleKey.D0);
		}

		private static async Task PerformAction(ConsoleKey option)
		{
			switch (option)
			{
				case ConsoleKey.D1:
					GetInstanceOfClassWithoutDependencies();
					break;
				case ConsoleKey.D2:
					GetInstanceOfClassWithDependency();
					break;
				case ConsoleKey.D3:
					MapInterfaceToImplementation();
					break;
				case ConsoleKey.D4:
					GetInstanceOfClassMappedThroughInterface();
					break;
				case ConsoleKey.D5:
					StoreWithoutDependenciesInstance();
					break;
				case ConsoleKey.D6:
					await GetInstanceOfClassWithoutDependenciesAsync();
					break;
				case ConsoleKey.D7:
					StoreWithoutDependenciesInstanceWithId();
					break;
				case ConsoleKey.D8:
					GetInstanceOfClassWithoutDependenciesUsingId();
					break;
				case ConsoleKey.R:
					RestartApplication();
					break;
				default:
					break;
			}
		}

		
		#region OptionHandling
		
		private static void GetInstanceOfClassWithDependency()
		{
			Container.Get<WithClassDependency>();
		}

		private static void GetInstanceOfClassWithoutDependencies()
		{
			if (!IsWithoutDependenciesStored)
			{
				LastWithoutDependencies = Container.Get<WithoutDependencies>();
			}
			else
			{
				ConsoleHelpers.WriteMessage(
					String.Format("Obtained instance of WithoutDependencies class with HashCode {0} from container.", 
					Container.Get<WithoutDependencies>().GetHashCode()),
					MessageKind.Action);
			}
		}

		private static void MapInterfaceToImplementation()
		{
			ConsoleHelpers.WriteMessage("Mapping IInterface to InterfaceImplementation.", MessageKind.Action);
			IsInterfaceMapped = true;
			Container.Map<IInterface>().To<InterfaceImplementation>().Commit();
		}

		private static void GetInstanceOfClassMappedThroughInterface()
		{
			if (IsInterfaceMapped)
			{
				Container.Get<IInterface>();
			}
			else
			{
				ConsoleHelpers.WriteMessage("Can't get instance of class through interface. Map (3) first.", MessageKind.Warning);
			}
		}

		private static void StoreWithoutDependenciesInstance()
		{
			if (LastWithoutDependencies != null)
			{
				Container.Store(LastWithoutDependencies);
				ConsoleHelpers.WriteMessage("Store last obtained instance of WithoutDependencies class in container.", MessageKind.Action);
				IsWithoutDependenciesStored = true;
			}
			else
			{
				ConsoleHelpers.WriteMessage("Can't store null instance. Get one (1) first.", MessageKind.Warning);
			}
		}

		private static async Task GetInstanceOfClassWithoutDependenciesAsync() 
		{
			await Container.GetAsync<WithoutDependencies>();
		}

		private static void StoreWithoutDependenciesInstanceWithId() 
		{
			if (LastWithoutDependencies != null)
			{
				Console.WriteLine(@"Id to register the instance with:");
				string id = Console.ReadLine();
				Container.Store(LastWithoutDependencies).WithId(id);
				Console.WriteLine();
			}
			else
			{
				ConsoleHelpers.WriteMessage("Can't store null instance. Get one (1) first.", MessageKind.Warning);
			}
		}

		private static void GetInstanceOfClassWithoutDependenciesUsingId()
		{
			Console.WriteLine(@"Registration Id:");
			string id = Console.ReadLine();

			ConsoleHelpers.WriteMessage(
				String.Format("Obtained instance of WithoutDependencies class with HashCode {0} from container.",
				Container.Get<WithoutDependencies>(id).GetHashCode()),
				MessageKind.Action);
		}
		
		private static void RestartApplication()
		{
			Container = new NeedleContainer();
			LastWithoutDependencies = null;
			IsInterfaceMapped = false;
			IsWithoutDependenciesStored = false;
			Console.Clear();
		}
		#endregion
		
		static void PrintMenu() 
		{
			StringBuilder menu = new StringBuilder();
			menu.Append("What operation would you like to perform? \n")
				.Append("1 - Get instance of class without dependencies.\n")
				.Append("2 - Get instance of class with class dependency. The dependency will be injected. \n")
				.Append("3 - Map IInterface to InterfaceImplementation. This enables you to get through IInterface. \n")
				.Append("4 - Get instance of class that implements interface through interface. \n")
				.Append("5 - Store last created instance of class without dependencies in container. \n")
				.Append("6 - Get instance of class without dependencies asynchronously.\n")
				.Append("7 - Store last created instance of class without dependencies in container using an Id. \n")
				.Append("8 - Get stored instance of class without dependencies using Id. \n")
				.Append("R - Restart application. \n")
				.Append("0 - Exit \n")
				.Append(">>> ");
			
			Console.Write(menu.ToString());
		}
	}
}
