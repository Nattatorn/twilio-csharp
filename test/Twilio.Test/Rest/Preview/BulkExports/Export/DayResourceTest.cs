using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Twilio.Clients;
using Twilio.Converters;
using Twilio.Exceptions;
using Twilio.Http;
using Twilio.Rest.Preview.BulkExports.Export;

namespace Twilio.Tests.Rest.Preview.BulkExports.Export 
{

    [TestFixture]
    public class DayTest : TwilioTest 
    {
        [Test]
        public void TestReadRequest()
        {
            var twilioRestClient = Substitute.For<ITwilioRestClient>();
            var request = new Request(
                HttpMethod.Get,
                Twilio.Rest.Domain.Preview,
                "/BulkExports/Exports/PathResourceType/Days",
                ""
            );
            twilioRestClient.Request(request).Throws(new ApiException("Server Error, no content"));

            try
            {
                DayResource.Read("PathResourceType", client: twilioRestClient);
                Assert.Fail("Expected TwilioException to be thrown for 500");
            }
            catch (ApiException) {}
            twilioRestClient.Received().Request(request);
        }

        [Test]
        public void TestReadResponse()
        {
            var twilioRestClient = Substitute.For<ITwilioRestClient>();
            twilioRestClient.AccountSid.Returns("ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            twilioRestClient.Request(Arg.Any<Request>())
                            .Returns(new Response(
                                         System.Net.HttpStatusCode.OK,
                                         "{\"days\": [{\"day\": \"2017-05-01\",\"size\": 1234,\"resource_type\": \"Calls\"}],\"meta\": {\"key\": \"days\",\"page_size\": 50,\"url\": \"https://preview.twilio.com/BulkExports/Exports/Calls/Days?PageSize=50&Page=0\",\"page\": 0,\"first_page_url\": \"https://preview.twilio.com/BulkExports/Exports/Calls/Days?PageSize=50&Page=0\",\"previous_page_url\": null,\"next_page_url\": null}}"
                                     ));

            var response = DayResource.Read("PathResourceType", client: twilioRestClient);
            Assert.NotNull(response);
        }
    }

}