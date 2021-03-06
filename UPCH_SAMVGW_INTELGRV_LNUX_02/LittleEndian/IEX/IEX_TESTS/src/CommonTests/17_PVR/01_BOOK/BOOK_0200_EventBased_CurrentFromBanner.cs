﻿/// <summary>
///  Script Name : BOOK_0200_EventBased_CurrentFromBanner.cs
///  Test Name   : BOOK-0200-Event Based-Current From Banner
///  TEST ID     : 59165
///  QC Version  : 4
/// -----------------------------------------------
///  Modified by : Bharath Pai
/// </summary>

using System;
using IEX.ElementaryActions.Functionality;
using IEX.Tests.Engine;

public class BOOK_0200 : _Test
{
    [ThreadStatic]
    private static _Platform CL;

    //Channels used by the test
    private static string serviceToBeCurrentRecorded; //The service to be currently recorded

    //Shared members between steps
    private static string currentEventRecording = "CURRENT_EVENT"; //The current event to be recorded

    //Constants used in the test
    private static class Constants
    {
        public const bool checkIfVideoIsPresent = true;
        public const bool checkFullVideoArea = true;
        public const int timeToCheckForVideo = 10;
    }

    #region Create Structure

    public override void CreateStructure()
    {
        this.AddStep(new PreCondition(), "Precondition: Get Channel Numbers From ini File & Sync.");
        this.AddStep(new Step1(), "Step 1: Select an ongoing event");
        this.AddStep(new Step2(), "Step 2: Book the event for recording");
        this.AddStep(new Step3(), "Step 3: Access My Recordings");

        //Get Client Platform
        CL = GetClient();
    }

    #endregion Create Structure

    #region Steps

    #region PreCondition

    private class PreCondition : _Step
    {
        public override void Execute()
        {
            StartStep();

            //Get Channel Values From ini File
            serviceToBeCurrentRecorded = CL.EA.GetValue("Short_SD_1");

            PassStep();
        }
    }

    #endregion PreCondition

    #region Step1

    private class Step1 : _Step
    {
        public override void Execute()
        {
            StartStep();

            //Tune to the service whose future event will be recorded
            res = CL.EA.ChannelSurf(EnumSurfIn.Live, serviceToBeCurrentRecorded);
            if (!res.CommandSucceeded)
            {
                FailStep(CL, res, "Failed to Tune to Channel - " + serviceToBeCurrentRecorded);
            }

            //Check for video
            res = CL.EA.CheckForVideo(Constants.checkIfVideoIsPresent, Constants.checkFullVideoArea, Constants.timeToCheckForVideo);
            if (!res.CommandSucceeded)
            {
                FailStep(CL, res, "Video not present!");
            }

            PassStep();
        }
    }

    #endregion Step1

    #region Step2

    private class Step2 : _Step
    {
        public override void Execute()
        {
            StartStep();

            //Book the current event for recording
            res = CL.EA.PVR.RecordCurrentEventFromBanner(currentEventRecording,MinTimeBeforeEvEnd:5);
            if (!res.CommandSucceeded)
            {
                FailStep(CL, res, "Failed to book current event for recording!");
            }

            PassStep();
        }
    }

    #endregion Step2

    #region Step3

    public class Step3 : _Step
    {
        public override void Execute()
        {
            StartStep();

            //Verify whether the recording is present in Recording list
            res = CL.EA.PVR.VerifyEventInArchive(currentEventRecording);
            if (!res.CommandSucceeded)
            {
                FailStep(CL, res, "Failed to find the recording in the My Recording List");
            }

            PassStep();
        }
    }

    #endregion Step3

    #endregion Steps

    #region PostExecute

    public override void PostExecute()
    {
    }

    #endregion PostExecute
}