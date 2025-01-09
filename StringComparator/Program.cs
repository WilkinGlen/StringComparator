using Newtonsoft.Json.Linq;

const string NotNullSpecifier = "¬¬NOTNULL¬¬";

var expectedResponse = "{\"name\": \"John\", \"age\": \"¬¬NOTNULL¬¬\", \"address\": {\"city\": \"New York\", \"zipcode\": \"¬¬NOTNULL¬¬\"}, \"hobbies\": [\"¬¬NOTNULL¬¬\", \"swimming\", \"coding\"]}";
var actualResponse = "{\"name\": \"John\", \"age\": 3, \"address\": {\"city\": \"New York\", \"zipcode\": \"WF44AB\"}, \"hobbies\": [\"reading\", \"swimming\", \"coding\"]}";
var expectedObject = JObject.Parse(expectedResponse);
var actualObject = JObject.Parse(actualResponse);
var areEqual = CompareJson(expectedObject, actualObject);
Console.WriteLine("Are JSON responses the same? " + areEqual);

static bool CompareJson(JObject expectedJsonObject, JObject actualJsonObject)
{
    if (!JObject.DeepEquals(expectedJsonObject, actualJsonObject))
    {
        var retVal = true;
        foreach (var property in expectedJsonObject.Properties())
        {
            var value1 = expectedJsonObject[property.Name];
            var value2 = actualJsonObject[property.Name];
            if (!JToken.DeepEquals(value1, value2))
            {
                if (value1 is JObject object1 && value2 is JObject object2)
                {
                    if (!CompareJson(object1, object2))
                    {
                        return false;
                    }
                }
                else if (value1 is JArray array1 && value2 is JArray array2)
                {
                    if (!CompareArrays(array1, array2))
                    {
                        return false;
                    }
                }
                else
                {
                    retVal = retVal && value1?.ToString() == NotNullSpecifier && !string.IsNullOrEmpty(value2?.ToString());
                }
            }
        }

        return retVal;
    }

    return true;
}

static bool CompareArrays(JArray array1, JArray array2)
{
    if (array1.Count != array2.Count)
    {
        return false;
    }

    var retVal = true;
    for (var i = 0; i < array1.Count; i++)
    {
        if (!JToken.DeepEquals(array1[i], array2[i]))
        {
            if (array1[i] is JObject object1 && array2[i] is JObject object2)
            {
                if (!CompareJson(object1, object2))
                {
                    return false;
                }
            }
            else if (array1[i] is JArray array_1 && array2[i] is JArray array_2)
            {
                if (!CompareArrays(array_1, array_2))
                {
                    return false;
                }
            }
            else
            {
                retVal = retVal && array1[i]?.ToString() == NotNullSpecifier && !string.IsNullOrEmpty(array2[i]?.ToString());
            }
        }
    }

    return retVal;
}

