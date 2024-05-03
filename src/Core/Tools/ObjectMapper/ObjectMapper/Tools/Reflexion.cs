using System.Reflection;

namespace ObjectMapper.Tools;

public class Reflexion {
    public static void SetProperty(object obj, string propertyName, object value) {
        var propertyInfo = obj.GetType().GetProperty(
            propertyName,
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
        );
        // Ensure the property exists and has a setter
        if (propertyInfo != null && propertyInfo.CanWrite) {
            // If the property has a private setter, it still needs to be accessed explicitly
            var setMethod = propertyInfo.GetSetMethod(true);
            if (setMethod != null)
                setMethod.Invoke(obj, new[] { value });
            else
                throw new InvalidOperationException(Error.PropertyDoesNotHaveSetter(propertyName).Message);
        }
        else {
            throw new InvalidOperationException(Error.PropertyDoesNotExist(propertyName).Message);
        }
    }
}