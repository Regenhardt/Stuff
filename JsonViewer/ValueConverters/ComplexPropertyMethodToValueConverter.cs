﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Newtonsoft.Json.Linq;

namespace JsonViewer.ValueConverters
{
    // This converter is only used by JProperty tokens whose Value is Array/Object
	internal class ComplexPropertyMethodToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
	        if (value == null || !(parameter is string methodName))
                return null;
            var methodInfo = value.GetType().GetMethod(methodName, new Type[0]);
            if (methodInfo == null)
                return null;
            var invocationResult = methodInfo.Invoke(value, new object[0]);
            var jTokens = (IEnumerable<JToken>) invocationResult;
            return jTokens.First().Children() ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only be used for one way conversion.");
        }
    }
}
