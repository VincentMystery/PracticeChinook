using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///     The contents that will be placed in an instance of this class
///     will ne place in an instance of this class will come from
///     user code within your application source (ie BLL business rule exceptions)
/// </summary>
[Serializable]
public class BusinessRuleException : Exception
{
    public List<string> RuleDetails { get; set; }
    public BusinessRuleException(string message, List<string> reasons)
        : base(message)
    {
        this.RuleDetails = reasons;
    }
}