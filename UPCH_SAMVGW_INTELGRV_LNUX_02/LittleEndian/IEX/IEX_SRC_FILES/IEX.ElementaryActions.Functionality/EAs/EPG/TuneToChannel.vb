﻿Imports FailuresHandler

Namespace EAImplementation

    ''' <summary>
    '''   Tune To Channel
    ''' </summary>
    Public Class TuneToChannel
        Inherits IEX.ElementaryActions.BaseCommand

        Private EPG As IEX.ElementaryActions.EPG.SF.UI
        Private _manager As IEX.ElementaryActions.Functionality.Manager
        Private _ChannelNumber As String
        Private _ReturnToLive As Boolean

        ''' <param name="ChannelNumber">The Channel Number</param>
        ''' <param name="ReturnToLive">If True Returnes To Live Before Tunning Else Not (For Tunning From Locked Channel)</param>
        ''' <param name="pManager">Manager</param>
        ''' <remarks>
        ''' Possible Error Codes:
        ''' <para>301 - DictionaryFailure</para> 
        ''' <para>322 - VerificationFailure</para> 
        ''' <para>328 - INIFailure</para> 
        ''' <para>334 - VideoNotPresent</para> 
        ''' <para>349 - ReturnToLiveFailure</para> 
        ''' <para>351 - SurfingFailure</para> 
        ''' <para>355 - TuneToChannelFailure</para> 
        ''' </remarks>
        Sub New(ByVal ChannelNumber As String, ByVal ReturnToLive As Boolean, ByVal pManager As IEX.ElementaryActions.Functionality.Manager)
            Me._ChannelNumber = ChannelNumber
            Me._ReturnToLive = ReturnToLive
            Me._manager = pManager
            EPG = Me._manager.UI
        End Sub

        Protected Overrides Sub Execute()
            Dim res As New IEXGateway.IEXResult

            If _ChannelNumber = "" Then
                ExceptionUtils.ThrowEx(New EAException(ExitCodes.NoValidParameters, "ChannelNumber Can't Be Empty"))
            End If

            If _ReturnToLive Then
                If Not EPG.Live.IsLive Then
                    EPG.Utils.ReturnToLiveViewing()
                End If
            End If

            EPG.Utils.StartHideFailures("Checking If Already Tunnned To " + Me._ChannelNumber)
            Try
                EPG.Live.VerifyChannelNumber(Me._ChannelNumber)
                _iex.ForceHideFailure()
                Exit Sub
            Catch ex As IEXException
                If ex.ExitCodeValue <> 112 Then
                    _iex.ForceHideFailure()
                    ExceptionUtils.ThrowEx(New IEXException(ex.ExitCodeValue, ex.ExitCodeName, "Failed To Check Channel Number In EpgInfo : " + ex.Message))
                End If
            Catch ex As EAException
            End Try
            _iex.ForceHideFailure()

            EPG.Live.TuningToChannel(Me._ChannelNumber, "", False)
            EPG.Live.VerifyChannelNumber(Me._ChannelNumber)

        End Sub

    End Class

End Namespace