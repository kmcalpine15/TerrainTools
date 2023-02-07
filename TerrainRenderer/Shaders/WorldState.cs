using System;
using OpenTK.Mathematics;
namespace TerrainRenderer.Shaders
{
	public class WorldVariable
	{
		public string Name { get; set; }
		public Type Type { get; set; }
		public object? Value { get; set; }

		public WorldVariable(string name, Type type)
		{
			Type = type;
			Name = name;
		}
	}

	/// <summary>
	/// Managed a list of world state to be used by shaders
	/// </summary>
	public class WorldState
	{
		private Dictionary<string, WorldVariable> _worldVariables;

		public WorldState(IEnumerable<WorldVariable> variableDefinitions)
		{
			_worldVariables = new Dictionary<string, WorldVariable>();

			foreach(var def in variableDefinitions)
			{
				if (_worldVariables.ContainsKey(def.Name))
				{
					throw new ArgumentException($"Duplicate variable defintion {def.Name}");
				}
				_worldVariables.Add(def.Name, def);
			}
		}
		

		public void SetValue<T>(string variableName, T value)
		{
			if (!_worldVariables.ContainsKey(variableName))
			{
				throw new ArgumentException($"Cannot set variable {variableName}, variable not found");
			}

			var variable = _worldVariables[variableName];
			var valueType = typeof(T);
			if (valueType != variable.Type)
			{
				throw new ArgumentException($"Variable {variableName} type mismatch. Variable Type: {variable.Type} Argument Type: {valueType}");
			}

			_worldVariables[variableName].Value = value;
		}

		public List<string> GetVariableNames()
		{
			return _worldVariables.Keys.ToList();
		}

		public WorldVariable GetVariable(string name)
		{
			return _worldVariables[name];
		}
	}
}
