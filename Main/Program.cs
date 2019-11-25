﻿using System;
using System.Collections.Generic;
using Transcriber;
using DatabaseController;
using DatabaseController.Data;
using SendGrid.Helpers.Mail;
using System.IO;
using twilio_caller;
using Scheduler;
using MeetingControllers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Main
{
    public static class Program
    {
        static void Main(string[] args)
        {
            /*Deserialize the init data for dialing in to meeting */
            InitData init = JsonConvert.DeserializeObject<InitData>(args[0]);

            if (!init.Debug)
                Run(init.MeetingAccessCode);

        }


        public static void Run(string accessCode)
        {
            // Set Authentication configurations
            var appConfig = Configurations.LoadAppSettings();

            // new dialer manager
            var dialManager = new twilio_caller.dialer.dialerManager(appConfig);
            // new recording download manager
            var recManager = new twilio_caller.dialer.RecordingManager(appConfig);

            // dial into and record the meeting
            var rid = dialManager.CallMeetingAsync(accessCode).Result;
            // download the recording to the file
            var recording = recManager.DownloadRecordingAsync(accessCode).Result;

            // Get email addresses for all users who are attending the meeting
            List<EmailAddress> invitedUsers = MeetingController.GetAttendeeEmails(accessCode);

            // transcribe the meeting
            Console.WriteLine(">\tBeginning Transcribing...");



            /*Load all the profiles by email address for registered users */
            var emails = EmailController.FromEmailAddressListToStringList(invitedUsers);

            /*Make controller for accessing registered user profiles in Azure Speaker Recognition endpoint*/
            RegistrationController regController = RegistrationController.BuildController(emails);

            /*Get registered profiles */
            List<User> voiceprints = regController.UserProfiles;

            TranscribeController transcribeController = new TranscribeController(recording, voiceprints);

            /*Do the transcription with speaker recognition*/
            if (transcribeController.Perform())
            {
                transcribeController.WriteTranscriptionFile();
                EmailController.SendMinutes(invitedUsers);
            }
            else
            {
                EmailController.SendEMail(invitedUsers, "Failed To Generate Meeting Transcription", "");
            }

            Console.WriteLine(">\tTasks Complete!");
        }


       








    }







}
