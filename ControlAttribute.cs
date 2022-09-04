using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[EntityIcon("")]
[EntityLabel("")]
[Description("Контроллер предназначен для .")]
public abstract class ControlAttribute: MyValidatableAttribute
{
    public abstract object CreateControl(object field);

}

