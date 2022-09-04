using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreModel.AttributeConventions
{
    public class InputDataConvention: Microsoft.EntityFrameworkCore.Metadata.Conventions.PropertyAttributeConventionBase<InputDateAttribute>
    {
        public InputDataConvention(ProviderConventionSetBuilderDependencies dependencies) : base(dependencies)
        {
        }

        protected override void ProcessPropertyAdded(
            IConventionPropertyBuilder propertyBuilder, 
            InputDateAttribute attribute, 
            MemberInfo clrMember, 
            IConventionContext context)
        {
           
        }
    }
}
