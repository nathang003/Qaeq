using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace Qaeq.Analyzers.Test
{
    public class XUnitVerifier : IVerifier
    {
        public XUnitVerifier() { }

        protected XUnitVerifier(string context)
        {
            Context = context;
        }

        protected string? Context { get; }

        public void Empty<T>(string collectionName, IEnumerable<T> collection)
        {
            Assert.Empty(collection);
        }

        public void Equal<T>(T expected, T actual, string? message = null)
        {
            if (message != null)
                Assert.True(EqualityComparer<T>.Default.Equals(expected, actual), message);
            else
                Assert.Equal(expected, actual);
        }

        [DoesNotReturn]
        public void Fail(string? message = null)
        {
            Assert.Fail(message ?? "Verification failed.");
        }

        public void False(bool assert, string? message = null)
        {
            if (message != null)
                Assert.True(!assert, message);
            else
                Assert.False(assert);
        }

        public void LanguageIsSupported(string language)
        {
            Assert.False(
                language != LanguageNames.CSharp && language != LanguageNames.VisualBasic,
                $"Unsupported Language: '{language}'");
        }

        public void NotEmpty<T>(string collectionName, IEnumerable<T> collection)
        {
            Assert.NotEmpty(collection);
        }

        public IVerifier PushContext(string context)
        {
            if (Context is null)
                return new XUnitVerifier(context);
            return new XUnitVerifier($"{Context} -> {context}");
        }

        public void True(bool assert, string? message = null)
        {
            if (message != null)
                Assert.True(assert, message);
            else
                Assert.True(assert);
        }

        public void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? equalityComparer = null, string? message = null)
        {
            var expectedList = expected.ToList();
            var actualList = actual.ToList();
            Equal(expectedList.Count, actualList.Count, $"Sequence lengths differ. {message}");
            for (int i = 0; i < expectedList.Count; i++)
            {
                var comparer = equalityComparer ?? EqualityComparer<T>.Default;
                Equal(true, comparer.Equals(expectedList[i], actualList[i]), $"Sequences differ at index {i}. {message}");
            }
        }
    }
}
