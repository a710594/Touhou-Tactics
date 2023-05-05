using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Nested Parameters", menuName = "Custom/Nested Parameters")]
public class NestedParameters : ScriptableObject
{
    [System.Serializable]
    public class Parameter
    {
        public string name;
        public string description;
        public object value;
        public List<Parameter> subParameters = new List<Parameter>();

        public Parameter(string _name, string _description, object _value)
        {
            name = _name;
            description = _description;
            value = _value;
        }

        public Parameter AddSubParameter(string _name, string _description, object _value)
        {
            Parameter subParameter = new Parameter(_name, _description, _value);
            subParameters.Add(subParameter);
            return subParameter;
        }
    }

    public List<Parameter> parameters = new List<Parameter>();

    public Parameter AddParameter(string _name, string _description, object _value)
    {
        Parameter parameter = new Parameter(_name, _description, _value);
        parameters.Add(parameter);
        return parameter;
    }
}
