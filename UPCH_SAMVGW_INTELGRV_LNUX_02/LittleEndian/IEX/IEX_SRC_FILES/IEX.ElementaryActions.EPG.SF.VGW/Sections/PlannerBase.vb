﻿Public Class PlannerBase
    Inherits IEX.ElementaryActions.EPG.SF.COGECO.PlannerBase

    Dim _uUI As IEX.ElementaryActions.EPG.SF.VGW.UI

    Sub New(ByVal _pIex As IEXGateway.IEX, ByVal UI As IEX.ElementaryActions.EPG.SF.UI)
        MyBase.New(_pIex, UI)
        _uUI = UI
    End Sub

End Class