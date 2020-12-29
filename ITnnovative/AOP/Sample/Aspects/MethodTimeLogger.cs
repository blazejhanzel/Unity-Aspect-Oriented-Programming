﻿using System;
using System.Diagnostics;
using System.Text;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Exectution.Arguments;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ITnnovative.AOP.Sample.Aspects
{
    /// <summary>
    /// This is an aspect that prints execution time of a method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodTimeLogger : Attribute, IMethodBoundaryAspect
    {
        /// <summary>
        /// Name of the Stopwatch variable
        /// </summary>
        public const string NAME_STOPWATCH_VARIABLE = "timer";
        
        public void OnMethodExit(MethodExecutionArguments args)
        {
            // Get Stopwatch variable
            var sw = args.GetVariable<Stopwatch>(NAME_STOPWATCH_VARIABLE);
            sw.Stop();
            
            // Create list of arguments for method
            var paramList = new StringBuilder();
            var num = 0;
            args.arguments.ForEach(a =>
            {
                if(num > 0)
                    paramList.Append(", ");
                paramList.Append(a.name);
                num++;
            });
            Debug.Log("[MethodTimeLogger] Method '" +
                      args.rootMethod.Name + "(" + paramList + ")' took " + sw.ElapsedMilliseconds + "ms.");
        }

        public void OnMethodEnter(MethodExecutionArguments args)
        {
            // Create stopwatch to measure time and start it
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            // Register variable in arguments
            args.AddVariable(NAME_STOPWATCH_VARIABLE, stopwatch);
        }
    }
}
