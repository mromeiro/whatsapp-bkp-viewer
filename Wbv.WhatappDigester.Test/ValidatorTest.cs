
using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Wbv.WhatsappDigester.Digester;
using Wbv.WhatsappDigester.Digester.Options;

namespace Wbv.WhatappDigester.Test
{
    public class ValidatorTest
    {
        private OptionsValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new OptionsValidator();
        }

        [Test]
        public void Validate_WhenFileDosNetExist_ShouldThrowException()
        {
            Action act  = () => _validator.Validate(new DigesterOptions() { Data = "Data/not_exist_test.zip", FileType = SourceData.Zip });

            act.Should().Throw<IOException>();
        }
    }
}