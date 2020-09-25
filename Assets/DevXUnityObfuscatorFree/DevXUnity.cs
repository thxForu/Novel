using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



#region DevXUnity
internal class DevXUnity
{
    //==========================================================================================
    //
    // The methods of this class are stubs,
    // When obfuscated, they are replaced by real functions.
    // The contents of this file can not be changed.
    //
    //==========================================================================================


    #region SelectLocalizationName
    /// <summary>
    /// Set the current localization language, for example "RU", "EN"
    /// This is a stub function
    /// </summary>
    /// <param name="localization_name"></param>
    /// <returns>result</returns>
    internal static bool SelectLocalizationName(string localization_name)
    {
        return true;
    }
    #endregion

    #region GetSelectedLocalizationName
    /// <summary>
    /// Get the current value of the selected localization language
    /// This is a stub function
    /// </summary>
    /// <returns>selected localization language</returns>
    internal static string GetSelectedLocalizationName()
    {
        return null;
    }
    #endregion

    #region LoadLocalizationFileFromText
    /// <summary>
    /// Set localization table via text block
    /// This is a stub function
    /// </summary>
    /// <param name="text_content"></param>
    /// <returns></returns>
    internal static bool LoadLocalizationFileFromText(string text_content)
    {
        return false;
    }
    #endregion

    #region >GetLocalizationKey
    /// <summary>
    /// Get the key (hash) of localization by text data
    /// This is a stub function
    /// </summary>
    /// <returns>The localization key.</returns>
    /// <param name="message">Message.</param>
    internal static int GetLocalizationKey(string message)
    {

        return 0;
    }
    #endregion

    #region >GetLocalizedText
    /// <summary>
    /// Get localized text on a key (hash)
    /// This is a stub function
    /// </summary>
    /// <returns>The localized text.</returns>
    /// <param name="localization_key">Localization key.</param>
    internal static string GetLocalizedText(int localization_key)
    {
        return ""+localization_key;
    }
    #endregion


    #region >Translate
    /// <summary>
    /// Set forced localization for a row or a reference to a string
    /// This is a stub function
    /// </summary>
    /// <param name="string_ref">string</param>
    /// <returns></returns>
    internal static string Translate(string string_ref)
    {
        return string_ref;
    }
    #endregion



    #region >NoStringEncrypt
    /// <summary>
    /// NoStringEncrypt
    /// This is a stub function
    /// </summary>
    /// <param name="string_ref">string</param>
    /// <returns></returns>
    internal static string NoStringEncrypt(string string_ref)
    {
        return string_ref;
    }
    #endregion

    #region >NoTranslate
    /// <summary>
    /// Exclude text from the list for localization
    /// This is a stub function
    /// </summary>
    /// <param name="string_ref">string</param>
    /// <returns></returns>
    internal static string NoTranslate(string string_ref)
    {
        return string_ref;
    }
    #endregion


    #region >AddToChangeLang
    /// <summary>
    /// Add event to change location
    /// This is a stub function
    /// </summary>
    /// <param name="fun">Action</param>
    internal static void AddToChangeLang(System.Action fun)
    {
    }
    #endregion

    #region >RemoveFromChangeLang
    /// <summary>
    /// Delete event on localization change
    /// This is a stub function
    /// </summary>
    /// <param name="fun">Action</param>
    internal static void RemoveFromChangeLang(System.Action fun)
    {
    }
    #endregion

    // ==================================

    #region LoadEncryptedResourceText
    /// <summary>
    /// Load encrypted text from resources
    /// This is a stub function
    /// </summary>
    /// <param name="resource_name">resource name</param>
    /// <param name="key">password</param>
    /// <returns>decrypted string</returns>
    internal static string LoadEncryptedResourceText(string resource_name, string key)
    {
        var txt = UnityEngine.Resources.Load(resource_name) as TextAsset;
        if (txt != null)
        {
            return txt.text;
        }
        return null;
    }
    #endregion

    #region LoadEncryptedResourceBinary
    /// <summary>
    ///  Load encrypted buffer from resources
    /// This is a stub function
    /// </summary>
    /// <param name="resource_name">resource name</param>
    /// <param name="key">password</param>
    /// <returns>decrypted buffer</returns>
    internal static byte[] LoadEncryptedResourceBinary(string resource_name, string key)
    {
        var txt = UnityEngine.Resources.Load(resource_name) as TextAsset;
        if (txt != null)
        {
            return txt.bytes;
        }
        return null;
    }
    #endregion


    #region LoadEncryptedResourceTextureDefault
    /// <summary>
    /// Load Encrypted Resource Texture by default password
    /// </summary>
    /// <param name="resource_name">resource name</param>
    /// <returns>decrypted texture</returns>
    internal static Texture2D LoadEncryptedResourceTextureDefault(string resource_name)
    {
        return LoadEncryptedResourceTexture(resource_name,null);
    }
    #endregion


    #region LoadEncryptedResourceTexture
    /// <summary>
    /// Load Encrypted Resource Texture
    /// This is a stub function
    /// </summary>
    /// <param name="resource_name">resource name</param>
    /// <param name="key">password, or null for default password</param>
    /// <returns>decrypted texture</returns>
    internal static Texture2D LoadEncryptedResourceTexture(string resource_name, string key)
    {
        var texture = UnityEngine.Resources.Load(resource_name) as Texture2D;
        return texture;
    }
    #endregion

    #region LoadEncryptedResourceTextureEx
    /// <summary>
    /// Load Encrypted Resource Texture Extended, for raw image (sources image file) encryption only
    /// This is a stub function
    /// </summary>
    /// <param name="resource_name">resource name</param>
    /// <param name="key">password, or null for default password</param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="format"></param>
    /// <param name="mipmap"></param>
    /// <param name="linear"></param>
    /// <returns>decrypted texture</returns>
    internal static Texture2D LoadEncryptedResourceTextureEx(string resource_name, string key, int width, int height, TextureFormat format, bool mipmap, bool linear)
    {
        var texture = UnityEngine.Resources.Load(resource_name) as Texture2D;
        return texture;
    }
    #endregion

    #region LoadEncryptedResourceAssetBundle
    /// <summary>
    /// This function is a special function stub
    /// This is a stub function
    /// </summary>
    /// <param name="resource_name">resource name</param>
    /// <param name="key">password</param>
    /// <returns>decrypted assertBundle</returns>
    internal static AssetBundle LoadEncryptedResourceAssetBundle(string resource_name, string key)
    {
        return UnityEngine.Resources.Load(resource_name) as AssetBundle;
    }
    #endregion

    //=================================


    #region CallStaticMethod
    internal static object CallStaticMethod(string method_name, params object[] args)
    {
        return CallMethodEx(type: null, instance: null, class_name: null, method_name: method_name, args: args);
    }
    #endregion

    #region CallInstanceMethod
    internal static object CallInstanceMethod(object instance, string method_name, params object[] args)
    {
        return CallMethodEx(type: null, instance: instance, class_name: null, method_name: method_name, args: args);
    }
    #endregion

    #region CallMethodEx
    internal static object CallMethodEx(Type type, object instance, string class_name, string method_name, params object[] args)
    {
        type = null;
        if (type == null)
        {
            if (instance != null)
            {
                type = instance.GetType();
            }

            if (class_name != null)
            {
                type = Type.GetType(class_name, throwOnError: false);
            }

            if (type == null)
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame frame = stackTrace.GetFrames()[1];
                var method = frame.GetMethod();
                string methodName = method.Name;
                Type methodsClass = method.DeclaringType;
                type = methodsClass;

                if (type == null)
                    return null;
            }
        }
        var m = type.GetMethod(method_name, (instance == null ? System.Reflection.BindingFlags.Static : System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static) | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (m == null)
            return null;



        return m.Invoke(m.IsStatic ? null : instance, args);
    }
    #endregion

    #region GetFieldEx
    internal static object GetFieldEx(Type type, object instance, string class_name, string field_name)
    {
        type = null;
        if (type == null)
        {
            if (instance != null)
            {
                type = instance.GetType();
            }

            if (class_name != null)
            {
                type = Type.GetType(class_name, throwOnError: false);
            }

            if (type == null)
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame frame = stackTrace.GetFrames()[1];
                var method = frame.GetMethod();
                string methodName = method.Name;
                Type methodsClass = method.DeclaringType;
                type = methodsClass;

                if (type == null)
                    return null;
            }
        }
        var m = type.GetField(field_name, (instance == null ? System.Reflection.BindingFlags.Static : System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static) | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (m == null)
            return null;


        return m.GetValue(m.IsStatic ? null : instance);
    }
    #endregion

    #region SetFieldEx
    internal static bool SetFieldEx(Type type, object instance, string class_name, string field_name, object value)
    {
        type = null;
        if (type == null)
        {
            if (instance != null)
            {
                type = instance.GetType();
            }

            if (class_name != null)
            {
                type = Type.GetType(class_name, throwOnError: false);
            }

            if (type == null)
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame frame = stackTrace.GetFrames()[1];
                var method = frame.GetMethod();
                string methodName = method.Name;
                Type methodsClass = method.DeclaringType;
                type = methodsClass;

                if (type == null)
                    return false;
            }
        }
        var m = type.GetField(field_name, (instance == null ? System.Reflection.BindingFlags.Static : System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static) | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (m == null)
            return false;


        m.SetValue(m.IsStatic ? null : instance, value);
        return true;
    }
    #endregion




    #region GetPropertyEx
    internal static object GetPropertyEx(Type type, object instance, string class_name, string property_name)
    {
        type = null;
        if (type == null)
        {
            if (instance != null)
            {
                type = instance.GetType();
            }

            if (class_name != null)
            {
                type = Type.GetType(class_name, throwOnError: false);
            }

            if (type == null)
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame frame = stackTrace.GetFrames()[1];
                var method = frame.GetMethod();
                string methodName = method.Name;
                Type methodsClass = method.DeclaringType;
                type = methodsClass;

                if (type == null)
                    return null;
            }
        }
        var m = type.GetProperty(property_name, (instance == null ? System.Reflection.BindingFlags.Static : System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static) | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (m == null)
            return null;


        return m.GetValue(m.GetGetMethod() != null && m.GetGetMethod().IsStatic ? null : instance, null);
    }
    #endregion

    #region SetFieldEx
    internal static bool SetPropertyEx(Type type, object instance, string class_name, string property_name, object value)
    {
        type = null;
        if (type == null)
        {
            if (instance != null)
            {
                type = instance.GetType();
            }

            if (class_name != null)
            {
                type = Type.GetType(class_name, throwOnError: false);
            }

            if (type == null)
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame frame = stackTrace.GetFrames()[1];
                var method = frame.GetMethod();
                string methodName = method.Name;
                Type methodsClass = method.DeclaringType;
                type = methodsClass;

                if (type == null)
                    return false;
            }
        }
        var m = type.GetProperty(property_name, (instance == null ? System.Reflection.BindingFlags.Static : System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static) | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (m == null)
            return false;

        m.SetValue(m.GetSetMethod() != null && m.GetSetMethod().IsStatic ? null : instance, value, new object[0]);
        return true;
    }
    #endregion
}
#endregion





static class DevXUnityExtentions
{
    internal static object CallMethodSecured<T>(this T instance, string method_name, params object[] args)
    {
        return DevXUnity.CallMethodEx(type: null, instance: instance, class_name: null, method_name: method_name, args: args);
    }
    internal static object GetFieldSecured<T>(this T instance, string field_name)
    {
        return DevXUnity.GetFieldEx(type: null, instance: instance, class_name: null, field_name: field_name);
    }
    internal static object SetFieldSecured<T>(this T instance, string field_name, object value)
    {
        return DevXUnity.SetFieldEx(type: null, instance: instance, class_name: null, field_name: field_name, value: value);
    }


    internal static object GetPropertySecured<T>(this T instance, string property_name)
    {
        return DevXUnity.GetPropertyEx(type: null, instance: instance, class_name: null, property_name: property_name);
    }
    internal static object SetPropertySecured<T>(this T instance, string property_name, object value)
    {
        return DevXUnity.SetPropertyEx(type: null, instance: instance, class_name: null, property_name: property_name, value: value);
    }
}



#region Obfuscate Attributes

/// <summary>
/// DevXUnity_ObfuscateAttribute
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Enum)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_ObfuscateAttribute : System.Attribute
{
}


/// <summary>
/// DevXUnity_ObfuscateWitchAllChildsAttribute 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Enum)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_ObfuscateWitchAllChildsAttribute : System.Attribute
{
}




/// <summary>
/// DoNotObfuscateClassOnlyAttribute 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Enum)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_DoNotObfuscateAttribute : System.Attribute
{
}

/// <summary>
/// DevXUnity_DoNotObfuscateClassWitchAllChildsAttribute
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_DoNotObfuscateClassWitchAllChildsAttribute : System.Attribute
{
}


/// <summary>
/// ControllFlowProtectAttribute 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_ControlFlowProtectAttribute : System.Attribute
{
}

/// <summary>
/// ControllFlowProtectHardLevelAttribute
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_ControlFlowProtectHardLevelAttribute : System.Attribute
{
}

/// <summary>
/// DevXUnity_ControlFlowProtectParanoiacLevelAttribute 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_ControlFlowProtectParanoiacLevelAttribute : System.Attribute
{
}


/// <summary>
/// Enable text strings for the class, method, properties
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_StringEncryptionAttribute : System.Attribute
{
}

/// <summary>
/// Enable text strings for the class, method, properties
/// </summary>
[DevXUnity_DeleteAfterObfuscate()]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DevXUnity_StringEncryptionStrongAttribute : System.Attribute
{
}


/// <summary>
/// Delete After Obfuscate the class, method, properties, fields, events
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Field)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_DeleteAfterObfuscate: System.Attribute
{
}


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Field)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_Delete_for_UserDemoMode: System.Attribute
{
}
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Field)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_Delete_for_NOT_UserDemoMode : System.Attribute
{
}


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Field)]
[DevXUnity_DeleteAfterObfuscate()]
public class DevXUnity_Delete_if_not_set_CustomOption: System.Attribute
{
    public string Option { get; set; }
}
#endregion


