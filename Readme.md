# Serializando y Deserializando JSON con CSharp 12.0 y .Net 8.0

Aspectos en C# desde la versión 8.0 hasta la versión 12.0 y cómo se reflejan en tu código.

1. **C# 8.0:**
   - **Operador de rango**: Utilizamos el operador de rango en `e.Message[..20]`. Este es un nuevo operador introducido en C# 8.0 que proporciona una sintaxis más limpia para trabajar con rangos y matrices.
   - **Tipos de referencia nullable**: Utilizanción de tipos de referencia nullable, como en `TemperatureInfo? forecast`. Esta es una característica introducida en C# 8.0 que permite a los desarrolladores expresar cuando una referencia puede ser nula, lo que ayuda a prevenir errores de NullReferenceException.
2. **C# 9.0:**
   - **Records**: Utilización de records, como en `public record TemperatureInfo`. Esta es una característica introducida en C# 9.0 que proporciona una sintaxis concisa para crear tipos de valor inmutables.
3. **Demostración de nuevas características:**
   - Uso del operador de rango, los tipos de referencia nullable y los records. 

## Best Practices for working with JSON in CSharp

1. **Use Strongly Typed Models**: Whenever possible, use strongly typed models to deserialize your JSON. This provides compile-time type checking and can make your code easier to understand and maintain.
2. **Error Handling**: Always handle potential errors when parsing JSON. This could be due to malformed JSON or missing keys. The `TryGetValue` method can be used to safely attempt to retrieve a value.
3. **Null Checking**: Always check for null values when accessing keys in a JSON object. In C#, you can use the null-conditional operator (`?.`) or the null-forgiving post-fix (`!.`) to safely access keys.
4. **Use Libraries**: Consider using libraries like Newtonsoft.Json or System.Text.Json for handling JSON. They provide many features like serialization/deserialization, LINQ to JSON, etc.
5. **Avoid Manual Parsing**: Avoid manually parsing JSON strings. It's error-prone and unnecessary with the aforementioned libraries.
6. **Formatting and Readability**: When storing or displaying JSON, consider formatting it with indentation. This makes it easier to read.
7. **Security**: Be aware of potential security implications when working with JSON, such as JavaScript injection attacks when using JSON within JavaScript code.
8. **Performance**: If performance is a concern, consider using the `Utf8JsonReader` and `Utf8JsonWriter` types for reading and writing UTF-8 encoded JSON text. These types offer high performance and low allocation reading and writing of UTF-8 encoded JSON text.

### Summary and check list

- [ ] **Use Strongly Typed Models**:
- [ ] **Error Handling**
- [ ] **Null Checking**: 
- [ ] **Use Libraries**:
- [ ] **Avoid Manual Parsing**
- [ ] **Formatting and Readability**:
- [ ] **Security**
- [ ] **Performance**

### Points to review:

```Powershell
Could you review if this code complais with the best practices in JSON with CSharp. Considering:
`Use Strongly Typed Models`,`Error Handling`,Null Checking`,`Use Libraries`, `Avoid Manual Parsing`,`Formatting and Readability`, `Security`, `Performance`
```

1. **Inferencia de tipos con `var` en declaraciones de objetos**: En C# 9.0, puedes usar `var` en lugar de especificar explícitamente el tipo al declarar e inicializar un objeto. Esto puede hacer que tu código sea más limpio y fácil de leer.
2. **Inicializadores de objetos simplificados**: En C# 9.0, puedes inicializar propiedades de un objeto directamente en la declaración del objeto, sin necesidad de un constructor.
3. **Registros (Records)**: En C# 9.0, los registros son una nueva característica que proporciona una sintaxis concisa para declarar clases de referencia que son inmutables por defecto.
4. **Mejoras en el manejo de excepciones**: En C# 10.0, puedes usar la declaración `throw` en expresiones.
5. **Mejoras en las asignaciones de variables**: En C# 10.0, puedes usar la declaración `var` en las asignaciones de variables.

### Result verifiying best practices

Code follows good practices for working with JSON in C#

1. **Use Strongly Typed Models**: You're using strongly typed models (`TemperatureInfo`, `TemperatureRange`) for deserialization which is a good practice.

2. **Error Handling**: You have error handling in place with a try-catch block around the file reading and deserialization process.

3. **Null Checking**: You're checking for null values before accessing properties of `forecast` and `temperatureRanges`, which is good.

4. **Use Libraries**: You're using the `System.Text.Json` library for JSON serialization and deserialization, which is recommended.

5. **Avoid Manual Parsing**: You're not manually parsing JSON, instead using `JsonSerializer`, which is good.

6. **Formatting and Readability**: Your code is generally well-formatted and readable. You're using `JsonSerializerOptions` to format the serialized JSON with indentation.

7. **Security**: Your code doesn't seem to have any obvious security issues related to JSON handling. You're not manually parsing JSON, which can be error-prone and potentially insecure.

8. **Performance**: `System.Text.Json` is a high-performance library for JSON serialization and deserialization. However, if your JSON files are very large, you might want to consider using a streaming approach instead of reading the entire file into memory with `File.ReadAllText`.

A few suggestions for improvement:

- Consider moving the `TemperatureInfo` and `TemperatureRange` classes, and the `DictionaryExtensions` class into separate files for better organization.

- The `TemperatureInfo` and `TemperatureRange` classes could be records if they are immutable.

- The `JsonOptions` class could be a static class since it only contains static members.

- Instead of using `WriteLine` to output JSON, consider returning JSON from your methods and outputting it at a higher level in your application. This would make your methods more reusable.

- Consider using `async` methods for file I/O and JSON serialization/deserialization to improve performance.

```CSharp
public class JsonOptionsBuilder
{
    private JsonSerializerOptions options;

    public JsonOptionsBuilder()
    {
        options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public JsonOptionsBuilder SetWriteIndented(bool value)
    {
        options.WriteIndented = value;
        return this;
    }

    public JsonOptionsBuilder SetPropertyNamingPolicy(JsonNamingPolicy policy = null)
    {
        options.PropertyNamingPolicy = policy ?? JsonNamingPolicy.Default;
        return this;
    }

    // Add more methods to set other options...

    public JsonSerializerOptions Build()
    {
        return options;
    }
}
```

### Using Builder pattern

```csharp
var builder = new JsonOptionsBuilder();
var options = builder
    .SetWriteIndented(true)
    .SetPropertyNamingPolicy(JsonNamingPolicy.CamelCase)
    .Build();
```

### Before of implementation builder pattner

```csharp
static class JsonOptions
{
    private static bool writeIndented = true;
    private static JsonSerializerOptions options = CreateOptions();

    private static JsonSerializerOptions CreateOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = writeIndented,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
    }

    public static JsonSerializerOptions GetOptions() => options;

    public static void SetOptions(JsonSerializerOptions value) => options = value;

    public static void SetWriteIndented(bool value)
    {
        writeIndented = value;
        options.WriteIndented = value; // Update WriteIndented in the current options
    }
}
```

### Put all in together 

```csharp
public class JsonOptionsBuilder
{
    private JsonSerializerOptions options;

    public JsonOptionsBuilder()
    {
        options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public JsonOptionsBuilder SetOptions(JsonSerializerOptions value)
    {
        options = value;
        return this;
    }

    public JsonOptionsBuilder SetWriteIndented(bool? value = null)
    {
        options.WriteIndented = value ?? true;
        return this;
    }

    public JsonOptionsBuilder SetPropertyNamingPolicy(JsonNamingPolicy policy = null)
    {
        options.PropertyNamingPolicy = policy ?? JsonNamingPolicy.Default;
        return this;
    }

    // Add more methods to set other options...

    public JsonSerializerOptions Build()
    {
        return options;
    }
    
    public JsonSerializerOptions GetOptions()
    {
        return options;
    }
}
```

## JSON serializing with `JsonNamingPolicy.SnakeCaseLower`

```json
{
  "date": "2024-05-01T00:00:00",
  "temperature": 25,
  "summary": "Hot",
  "dates_available": [
    "2024-05-01T00:00:00",
    "2024-05-02T00:00:00"
  ],
  "temperature_ranges": {
    "Cold": {
      "high": 23,
      "low": -9
    },
    "Hot": {
      "high": 51,
      "low": 32
    }
  }
}
```

### JSON deserializing with `JsonNamingPolicy.CamelCase`

```json
{
  "Date": "2024-05-01T00:00:00",
  "Temperature": 25,
  "Summary": "Hot",
  "DatesAvailable": [
    "2024-05-01T00:00:00",
    "2024-05-02T00:00:00"
  ],
  "TemperatureRanges": {
    "Cold": {
      "High": 23,
      "Low": -9
    },
    "Hot": {
      "High": 51,
      "Low": 32
    }
  }
}
```

More information

[How to customize property names and values with System.Text.Json](https://bit.ly/3y0DTmR)

[How to serialize properties of derived classes with System.Text.Json - .NET8.0](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/polymorphism?pivots=dotnet-8-0)

[What's new in System.Text.Json in .NET 8 - .NET Blog (microsoft.com)](https://devblogs.microsoft.com/dotnet/system-text-json-in-dotnet-8/)



### Code evolution

```csharp
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TextJson
{
    public interface IShapeVisitor
    {
        void Visit(Circle circle);
        void Visit(Rectangle rectangle);
        void Visit(Square square);
        void Visit(Triangle triangle);
    }

    public abstract class Shape
    {
        public abstract void Accept(IShapeVisitor visitor);
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }

        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Similar Accept methods for Rectangle, Square, and Triangle...

    public class ShapeConverterWithTypeDiscriminator : JsonConverter<Shape>, IShapeVisitor
    {
        private Utf8JsonWriter _writer;
        private JsonSerializerOptions _options;

        public override Shape Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Similar to original...
        }

        public override void Write(Utf8JsonWriter writer, Shape shape, JsonSerializerOptions options)
        {
            _writer = writer;
            _options = options;

            writer.WriteStartObject();
            shape.Accept(this);
            writer.WriteEndObject();
        }

        public void Visit(Circle circle)
        {
            _writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Circle);
            _writer.WriteNumber("Radius", circle.Radius);
        }

        // Similar Visit methods for Rectangle, Square, and Triangle...
    }
}
```

Este código se puede mejorar utilizando el patrón de diseño Visitor para desacoplar el código de serialización/deserialización de las clases de formas (Shape). Esto permitirá que las clases de formas sean más limpias y que el código de serialización/deserialización sea más fácil de mantener.

## Evolucionando código antiguo

[BillyClassTime/System.Text.JsonInNET6.0 (github.com)](https://github.com/BillyClassTime/System.Text.JsonInNET6.0)

Aquí está el código mejorado del proyecto de hace 3 años incorporando un patrón de diseño `Visitor`

```csharp
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TextJson
{
    public interface IShapeVisitor
    {
        void Visit(Circle circle);
        void Visit(Rectangle rectangle);
        void Visit(Square square);
        void Visit(Triangle triangle);
    }

    public abstract class Shape
    {
        public abstract void Accept(IShapeVisitor visitor);
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }

        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Similar Accept methods for Rectangle, Square, and Triangle...

    public class ShapeConverterWithTypeDiscriminator : JsonConverter<Shape>, IShapeVisitor
    {
        private Utf8JsonWriter _writer;
        private JsonSerializerOptions _options;

        public override Shape Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Similar to original...
        }

        public override void Write(Utf8JsonWriter writer, Shape shape, JsonSerializerOptions options)
        {
            _writer = writer;
            _options = options;

            writer.WriteStartObject();
            shape.Accept(this);
            writer.WriteEndObject();
        }

        public void Visit(Circle circle)
        {
            _writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Circle);
            _writer.WriteNumber("Radius", circle.Radius);
        }

        // Similar Visit methods for Rectangle, Square, and Triangle...
    }
}
```

Este código utiliza el patrón de diseño Visitor para desacoplar el código de serialización/deserialización de las clases de formas. Cada forma tiene un método `Accept` que acepta un `IShapeVisitor`. El `ShapeConverterWithTypeDiscriminator` implementa `IShapeVisitor`, y cuando se llama a `shape.Accept(this)`, se llama al método `Visit` apropiado en `ShapeConverterWithTypeDiscriminator`, que realiza la serialización.