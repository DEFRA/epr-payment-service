using System.Reflection;
using Microsoft.Data.SqlClient;

namespace EPR.Payment.Service.Data.UnitTests.TestHelpers
{
    internal static class SqlExceptionFactory
    {
        public static SqlException Create(int errorNumber)
        {
            var collectionCtor = typeof(SqlErrorCollection)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null)
                ?? throw new InvalidOperationException("SqlErrorCollection ctor not found.");
            var collection = (SqlErrorCollection)collectionCtor.Invoke(null);

            var errorCtor = typeof(SqlError)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault(c => c.GetParameters().Length >= 7 && c.GetParameters()[0].ParameterType == typeof(int))
                ?? throw new InvalidOperationException("SqlError ctor not found.");

            var paramCount = errorCtor.GetParameters().Length;
            object?[] args = paramCount switch
            {
                9 => new object?[] { errorNumber, (byte)0, (byte)10, "server", "Forced unique-violation for test.", "proc", 0, (uint)0, null },
                8 => new object?[] { errorNumber, (byte)0, (byte)10, "server", "Forced unique-violation for test.", "proc", 0, (uint)0 },
                7 => new object?[] { errorNumber, (byte)0, (byte)10, "server", "Forced unique-violation for test.", "proc", 0 },
                _ => throw new InvalidOperationException($"Unexpected SqlError ctor with {paramCount} params."),
            };
            var error = (SqlError)errorCtor.Invoke(args);

            var addMethod = typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("SqlErrorCollection.Add not found.");
            addMethod.Invoke(collection, new object[] { error });

            var createMethod = typeof(SqlException).GetMethod(
                "CreateException",
                BindingFlags.NonPublic | BindingFlags.Static,
                null,
                new[] { typeof(SqlErrorCollection), typeof(string) },
                null)
                ?? throw new InvalidOperationException("SqlException.CreateException not found.");
            return (SqlException)createMethod.Invoke(null, new object[] { collection, "7.0.0" })!;
        }
    }
}
