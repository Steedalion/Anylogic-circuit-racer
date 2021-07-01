using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class MessageParserTests
    {
        private MemoryStream myStream;
        private StreamWriter writer;
        private StreamReader reader;

        [SetUp]
        public void CreateStreams()
        {
            myStream = new MemoryStream();
            writer = new StreamWriter(myStream);
            reader = new StreamReader(myStream, Encoding.ASCII);
        }

        [Test]
        public void StreamInitialization()
        {
            WriteToBuffer("Hello");
            WriteToBuffer("Hello");
            myStream.Position = 0;
            Assert.AreEqual("Hello", reader.ReadLine(), "First Time");
            Assert.AreEqual("Hello", reader.ReadLine(), "second Time");
        }

        private void WriteToBuffer(string msg)
        {
            writer.WriteLine(msg);
            writer.Flush();
        }

        [Test]
        public void SendHelloToMessage()
        {
            TestConnection.StoreMessage messageHandler = new TestConnection.StoreMessage();
            messageHandler.SetStream(reader);
            WriteToBuffer("Hello");
            ResetBuffer();
            messageHandler.ReadAndParse();
            Assert.AreEqual("Hello", messageHandler.lastMessage);
        }

        private void ResetBuffer()
        {
            myStream.Position = 0;
        }

        [Test]
        public void ReoccuringSingle()
        {
            RecurMessageStore handler = new RecurMessageStore();
            handler.SetStream(reader);
            List<string> expectedMessages = new List<string>();
            expectedMessages.Add("Move here");
            foreach (string message in expectedMessages)
            {
                WriteToBuffer(message);
                ResetBuffer();
                handler.ReadAndParse();
            }

            List<string> messages = handler.GetMessages();
            for (int i = 0; i < expectedMessages.Count; i++)
            {
                Assert.AreEqual(expectedMessages[i], messages[i], messages.Count + " " + i);
            }
        }


        [Test]
        public void ReoccuringMessageParsing()
        {
            RecurMessageStore handler = new RecurMessageStore();
            handler.SetStream(reader);
            List<string> expectedMessages = new List<string>();
            expectedMessages.Add("Move here");
            expectedMessages.Add("Move there");
            expectedMessages.Add("Move everywhere");

            foreach (string message in expectedMessages)
            {
                WriteToBuffer(message);
            }

            ResetBuffer();

            handler.Start();
            List<string> messages = handler.GetMessages();
            for (int i = 0; i < expectedMessages.Count; i++)
            {
                Assert.AreEqual(expectedMessages[i], messages[i], messages.Count + " " + i);
            }
        }

        [Test]
        public void poReoccuringMessageOrder()
        {
            RecurMessageStore handler = new RecurMessageStore();
            handler.SetStream(reader);
            List<string> expectedMessages = new List<string>();

            expectedMessages.Add("Move here");
            expectedMessages.Add("Move there");
            expectedMessages.Add("Move everywhere");

            foreach (string message in expectedMessages)
            {
                WriteToBuffer(message);
                ResetBuffer();
                handler.ReadAndParse();
            }

            List<string> messages = handler.GetMessages();

            for (int i = 0; i < messages.Count; i++)
            {
                Debug.Log("actual: " + messages[i]);
                Debug.Log("expected: " + expectedMessages[i]);
            }

            for (int i = 0; i < expectedMessages.Count; i++)
            {
                Assert.AreEqual(expectedMessages[i], messages[i], messages.Count + " " + i);
            }
        }
    }

    public class RecurMessageStore : MessageHandler
    {
        private List<string> messages = new List<string>();

        public override string ReadAndParse()
        {
            string msg = myStream.ReadLine();
            Debug.Log(msg);
            messages.Add(msg);
            return msg;
        }

        public List<string> GetMessages()
        {
            return messages;
        }

        public void Start()
        {
            while (!myStream.EndOfStream)
            {
                ReadAndParse();
            }
        }
    }
}