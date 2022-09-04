using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ThrowableExtensions
{
    public static string ToHTML(this Exception Ex){
        return Expression.Interpolate(
            @"<div class='alert alert-secondary mt-4' role='alert'>
                
                <summary>
                    <strong> {{ GetType().Name }}
                        {{Message}}
                    </strong>
                    <div class='text text-info'>{{Source}}</div>
                    <div class='text text-warning'>{{TargetSite}}</div>   
                    <div>{{StackTrace}}
                    </div>
                    <a target='_blank'
                        class='font-weight-bold'
                        href='https://go.microsoft.com/fwlink/?linkid=2137813'></a>
                </summary>
                
            </div>", Ex).ToString();
    }

    public static void Fix(this Exception ex)
    {
        //WinSysAPI.InfoDialog("Источник",ex.Source);
        //WinSysAPI.EditTextFile(ex.Source);

    }
}