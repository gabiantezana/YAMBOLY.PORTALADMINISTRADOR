using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;

public static class ConvertHelper
{
    //TODO: ELIMINAR CAMPO DE USUARIO CREADO EN CLASE DIRECCIÓN
    #region Int32 Convert Helper
    /// <summary>
    /// Covierte un objeto a un entero tipo Int32
    /// </summary>
    /// <param name="val"></param>
    /// <param name="def"></param>
    /// <returns>Int32</returns>
    public static Int32 ToInteger(this object val, Int32 def)
    {
        try
        {
            Int32 reval = 0;

            if (Int32.TryParse(val.ToString(), out reval))
                return reval;

        }
        catch (Exception)
        {

        }

        return def;
    }

    public static DateTime ToDateTime(DateTime? fecha, DateTime? hora)
    {
        if (fecha.HasValue)
        {
            if (hora.HasValue)
            {
                String strFecha = fecha.ToSafeString().Substring(0, 11) + hora.Value.TimeOfDay;
                DateTime parseFecha = DateTime.Parse(strFecha);
                return parseFecha;
            }
            else
            {
                return fecha.Value;
            }
        }
        else
            return DateTime.Now;
    }

    public static DateTime ToDateTime_(DateTime? fecha, TimeSpan? hora)
    {
        if (fecha.HasValue)
        {
            if (hora.HasValue)
            {
                String strFecha = fecha.ToSafeString().Substring(0, 11) + hora.Value;
                DateTime parseFecha = DateTime.Parse(strFecha);
                return parseFecha;
            }
            else
            {
                return fecha.Value;
            }
        }
        else
            return DateTime.Now;
    }

    public static Int32 ToInteger(this object val)
    {
        return ConvertHelper.ToInteger(val, 0);
    }
    #endregion

    #region String Convert Helper
    /// <summary>
    /// Retorna un String de manera segura evitando errores de objetos nulos
    /// </summary>
    /// <param name="val"></param>
    /// <returns>String</returns>
    public static String ToSafeString(this object val)
    {
        return (val ?? String.Empty).ToString();
    }

    public static string FirstCharToUpper(this string input)
    {
        var finalText = string.Empty;
        input = input.ToSafeString().ToLower();
        foreach (var word in input.Split(' ').ToList())
        {
            if (word.Length > 0)
                finalText += word.First().ToString().ToUpper() + (word.Length > 1 ? word.Substring(1) : "") + " ";
        }

        return finalText.Trim();
    }

    static string[] pats3 = { "é", "É", "á", "Á", "í", "Í", "ó", "Ó", "ú", "Ú" };
    static string[] repl3 = { "e", "E", "a", "A", "i", "I", "o", "O", "u", "U" };
    static Dictionary<string, string> _var = null;
    static Dictionary<string, string> dict
    {
        get
        {
            if (_var == null)
            {
                _var = pats3.Zip(repl3, (k, v) => new { Key = k, Value = v }).ToDictionary(o => o.Key, o => o.Value);
            }

            return _var;
        }
    }
    public static string RemoveAccent(string text)
    {
        // using Zip as a shortcut, otherwise setup dictionary differently as others have shown
        //var dict = pats3.Zip(repl3, (k, v) => new { Key = k, Value = v }).ToDictionary(o => o.Key, o => o.Value);

        //string input = "åÅæÆäÄöÖøØèÈàÀìÌõÕïÏ";
        string pattern = String.Join("|", dict.Keys.Select(k => k)); // use ToArray() for .NET 3.5
        string result = Regex.Replace(text, pattern, m => dict[m.Value]);

        //Console.WriteLine("Pattern: " + pattern);
        //Console.WriteLine("Input: " + text);
        //Console.WriteLine("Result: " + result);

        return result;
    }
    #endregion

    #region Byte Array Helper
    public static byte[] GetBytes(this String str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
    #endregion

    #region CustomCast
    public static T Cast<T>(this Object myobj)
    {
        Type objectType = myobj.GetType();
        Type target = typeof(T);
        var x = Activator.CreateInstance(target, false);
        var z = from source in objectType.GetMembers().ToList()
                where source.MemberType == MemberTypes.Property
                select source;
        var d = from source in target.GetMembers().ToList()
                where source.MemberType == MemberTypes.Property
                select source;
        List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
           .ToList().Contains(memberInfo.Name)).ToList();
        PropertyInfo propertyInfo;
        object value;
        foreach (var memberInfo in members)
        {
            propertyInfo = typeof(T).GetProperty(memberInfo.Name);
            if (myobj.GetType().GetProperty(memberInfo.Name) != null)
            {
                value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);
                var elems = value as IList;

                if (value != null && !value.GetType().Namespace.Contains("System.Generic"))
                    propertyInfo.SetValue(x, value, null);

            }
        }
        return (T)x;
    }

    public static T CastJson<T>(this Object myobj)
    {
        var json = JsonConvert.SerializeObject(myobj, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        return JsonConvert.DeserializeObject<T>(json);
    }

    #endregion

    public static Object CopyAToB(Object a, Object b)
    {
        try
        {
            // copy fields
            //Para tipos de objeto heredados usar a.GetType().BaseType:
            var typeOfA = a.GetType();
            var typeOfB = b.GetType();

            foreach (var fieldOfA in typeOfA.GetFields())
            {
                try
                {
                    var fieldOfB = typeOfB.GetField(fieldOfA.Name);
                    fieldOfB.SetValue(b, fieldOfA.GetValue(a));
                }
                catch (Exception ex)
                {

                }
            }
            // copy properties
            foreach (var propertyOfA in typeOfA.GetProperties())
            {
                try
                {
                    var propertyOfB = typeOfB.GetProperty(propertyOfA.Name);
                    propertyOfB.SetValue(b, propertyOfA.GetValue(a));
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
            return b;
        }

        return b;
    }

    public partial class JsonEntityComparer : IEqualityComparer<JsonEntityTwoString>
    {
        public bool Equals(JsonEntityTwoString b1, JsonEntityTwoString b2)
        {
            if (b1.id == b2.id && b1.text == b2.text)
                return true;
            else
                return false;
        }

        public int GetHashCode(JsonEntityTwoString bx)
        {
            return bx.id.GetHashCode() ^ bx.text.GetHashCode();
        }
    }

    #region GetMemberName

    public static string GetMemberName<T>(Expression<Func<T, object>> expression)
    {
        return GetMemberName(expression.Body);
    }
    private static string GetMemberName(Expression expression)
    {
        if (expression == null)
        {
            throw new ArgumentException("Expresión can not be null");
        }

        if (expression is MemberExpression)
        {
            // Reference type property or field
            var memberExpression = (MemberExpression)expression;
            return memberExpression.Member.Name;
        }

        if (expression is MethodCallExpression)
        {
            // Reference type method
            var methodCallExpression = (MethodCallExpression)expression;
            return methodCallExpression.Method.Name;
        }

        if (expression is UnaryExpression)
        {
            // Property, field of method returning value type
            var unaryExpression = (UnaryExpression)expression;
            return GetMemberName(unaryExpression);
        }

        throw new ArgumentException("Error");
    }
    private static string GetMemberName(UnaryExpression unaryExpression)
    {
        if (unaryExpression.Operand is MethodCallExpression)
        {
            var methodExpression = (MethodCallExpression)unaryExpression.Operand;
            return methodExpression.Method.Name;
        }

        return ((MemberExpression)unaryExpression.Operand).Member.Name;
    }

    public static string GetMemberName<T>(this T instance, Expression<Func<T, object>> expression)
    {
        return GetMemberName(expression.Body);
    }

    #endregion
}