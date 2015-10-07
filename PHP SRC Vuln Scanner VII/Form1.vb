Imports System.IO

Public Class Form1
    Dim BackUpCode As String
    Dim MyName As String
    Private Sub BtnAnalyze_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAnalyze.Click
        Dim CurrentLine As Integer = 0
        Dim LineToCheck As String

        With RichTextBox1
            .SelectionStart = .TextLength
            .SelectionColor = .ForeColor
            If ChkFolder.Checked = True Then
                '  .AppendText(Environment.NewLine & Environment.NewLine & "Checking For Vulns!" & Environment.NewLine & TxtFiles.Lines(0))
            Else
                .AppendText(Environment.NewLine & Environment.NewLine & "Checking For Vulns!" & Environment.NewLine & TXTPath.Text)
            End If

        End With
DoAgain:
        Dim FinishLine As Integer = TXTPHP.Lines.Count
        Try
            LineToCheck = TXTPHP.Lines(CurrentLine)
        Catch ex As Exception
            CurrentLine = FinishLine + 1
        End Try
        Me.Text = MyName & " Working On Line #: " & CurrentLine & " Total Lines To Check: " & FinishLine
        Call CheckForVulns(LineToCheck, CurrentLine)
        If ChkFolder.Checked = True Then
            If CurrentLine = FinishLine + 1 Then
                Try
                    Call LoadPHP(TxtFiles.Lines(0))
                Catch ex As Exception
                    With RichTextBox1
                        .SelectionStart = .TextLength
                        .SelectionColor = .ForeColor
                        .AppendText(Environment.NewLine & Environment.NewLine & "Done!")
                        Me.Text = MyName
                        Exit Sub
                    End With
                End Try
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = .ForeColor
                    .AppendText(Environment.NewLine & Environment.NewLine & "Checking For Vulns!" & Environment.NewLine & TxtFiles.Lines(0))
                End With
                TxtFiles.Text = Replace(TxtFiles.Text, TxtFiles.Lines(0) & vbCrLf, "")
                LabelFileCount.Text = TxtFiles.Lines.Count & " Files To Check"
                CurrentLine = 0
            End If
        Else
            If CurrentLine = FinishLine + 1 Then
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = .ForeColor
                    .AppendText(Environment.NewLine & Environment.NewLine & "Done!")
                    Me.Text = MyName
                    Exit Sub
                End With

                CurrentLine = 0
            End If
        End If
        CurrentLine = CurrentLine + 1

        GoTo DoAgain
    End Sub

    Private Sub CheckForVulns(ByVal LineToCheck As String, ByVal LineNumber As Integer)
        Dim NextLine As String
        Try
            NextLine = TXTPHP.Lines(LineNumber + 1)
        Catch ex As Exception
            nextline = ""
        End Try
        If InStr(LineToCheck, "$_GET['") Then
            '  NextLine = TextBox1.Lines(0)

            'check for RFI
            If InStr(NextLine, "include $") Then
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = Color.Red
                    .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: RFI Found! On Line # On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                End With
            End If
            Application.DoEvents()
            'check for LFI
            If InStr(NextLine, "include '") Then
                '  TextBox2.Text = TextBox2.Text & "Possable Vuln: LFI Found!" & vbCrLf
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = Color.Red
                    .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: LFI Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                End With
            End If
            'check for SQLi
            Application.DoEvents()
            If InStr(NextLine, "= mysql_") Then
                ' TextBox2.Text = TextBox2.Text & "Possable Vuln: SQLi Found!" & vbCrLf
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = Color.Red
                    .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: SQLi Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                End With
            End If
            Application.DoEvents()
            'Check For RCE 1
            If InStr(NextLine, "system($") Then
                ' TextBox2.Text = TextBox2.Text & "Possable Vuln: Remote Code Execution Found!" & vbCrLf
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = Color.Red
                    .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: Remote Code Execution Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                End With
            End If
            Application.DoEvents()
            'Check For RCE 2
            If InStr(NextLine, "eval($") Then
                ' TextBox2.Text = TextBox2.Text & "Possable Vuln: Remote Code Execution Found!" & vbCrLf
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = Color.Red
                    .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: Remote Code Execution Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                End With
            End If
            Application.DoEvents()
            'Check for XSS
            If CheckBox1.Checked = True Then
            Else
                If InStr(NextLine, "print $") Then

                    'TextBox2.Text = TextBox2.Text & "Possable Vuln: XSS Found!" & vbCrLf
                    With RichTextBox1
                        .SelectionStart = .TextLength
                        .SelectionColor = Color.Red
                        .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: XSS Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                    End With
                End If
            End If
            Application.DoEvents()
            If InStr(NextLine, "include($") Then
                ' TextBox2.Text = TextBox2.Text & "Possable Vuln: PHP Code Execution/File Inclusion" & vbCrLf
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = Color.Red
                    .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: PHP Code Execution/File Inclusion On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                End With
            End If
            Application.DoEvents()
        Else

        End If


        If InStr(LineToCheck, "$_POST['") Then
            If InStr(BackUpCode, "mysql_real_escape_string") Then
            Else
                If InStr(BackUpCode, "addslashes") Then
                Else
                    ' TextBox2.Text = TextBox2.Text & "Possable Vuln: SQLi" & vbCrLf
                    With RichTextBox1
                        .SelectionStart = .TextLength
                        .SelectionColor = Color.Red
                        .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: SQLi On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                    End With
                End If

            End If
        End If
        If CheckBox1.Checked = True Then
        Else
            If InStr(LineToCheck, "echo") Then
                If InStr(BackUpCode, "strip_tags") Then
                Else
                    ' TextBox2.Text = TextBox2.Text & "Possable Vuln: XSS Found!" & vbCrLf
                    With RichTextBox1
                        .SelectionStart = .TextLength
                        .SelectionColor = Color.Red
                        .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: XSS Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                    End With
                End If
            End If
        End If

        If InStr(LineToCheck, "$_SERVER['") Then
            If InStr(BackUpCode, "addslashes") Then
            Else
                ' TextBox2.Text = TextBox2.Text & "Possable Vuln: LFI Found!" & vbCrLf
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = Color.Red
                    .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: LFI Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                End With
            End If
        End If

        If InStr(LineToCheck, "require($") Then
            ' TextBox2.Text = TextBox2.Text & "Possable Vuln: LFI Found!" & vbCrLf
            With RichTextBox1
                .SelectionStart = .TextLength
                .SelectionColor = Color.Red
                .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: LFI Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
            End With
        End If

        If InStr(LineToCheck, "include") Then
            If InStr(LineToCheck, "'.php'") Then GoTo MoveOn
            If InStr(BackUpCode, "addslashes") Then
            Else
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = Color.Red
                    .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: LFI Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                End With
            End If

MoveOn:
        End If
        If InStr(LineToCheck, "$_COOKIE[") Then
            ' TextBox2.Text = TextBox2.Text & "Possable Vuln: Cookie Bypass (Could be false)" & vbCrLf
            With RichTextBox1
                .SelectionStart = .TextLength
                .SelectionColor = Color.Red
                .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: Cookie Bypass (Could be false) On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
            End With
        End If

        If InStr(LineToCheck, "readfile($") Then
            ' TextBox2.Text = TextBox2.Text & "Possable Vuln: Read Files!" & vbCrLf
            With RichTextBox1
                .SelectionStart = .TextLength
                .SelectionColor = Color.Red
                .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: Read Files! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
            End With
        End If

        If InStr(LineToCheck, "fputs(") Then
            If InStr(BackUpCode, "escapeshellarg") Then
            Else
                If InStr(BackUpCode, "escapeshellcmd") Then
                Else
                    '  TextBox2.Text = TextBox2.Text & "Possable Vuln: Remote Code Execution!" & vbCrLf
                    With RichTextBox1
                        .SelectionStart = .TextLength
                        .SelectionColor = Color.Red
                        .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: Remote Code Execution! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
                    End With
                End If

            End If
        End If

        Application.DoEvents()
        If InStr(LineToCheck, "require_once($") Then
            ' TextBox2.Text = TextBox2.Text & "Possable Vuln: LFI Found!" & vbCrLf
            With RichTextBox1
                .SelectionStart = .TextLength
                .SelectionColor = Color.Red
                .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: LFI Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
            End With
        End If
        Application.DoEvents()
        If InStr(LineToCheck, "include($_GET['") Then
            'TextBox2.Text = TextBox2.Text & "Possable Vuln: LFI Found!" & vbCrLf
            With RichTextBox1
                .SelectionStart = .TextLength
                .SelectionColor = Color.Red
                .AppendText(Environment.NewLine & Environment.NewLine & "Possable Vuln: LFI Found! On Line #: " & LineNumber & vbCrLf & " VulnCode: " & LineToCheck)
            End With
        End If
        Application.DoEvents()
        BackUpCode = LineToCheck
    End Sub

    Public Function FileNameWithoutExtension(ByVal FullPath As String) As String

        Return System.IO.Path.GetFileName(FullPath)

    End Function

    Private Sub ParseShitz()
        txtfiles.Text = ""
        ' First create a FolderBrowserDialog object


        '------------------
        Dim list As List(Of String) = GetFilesRecursive(TXTPath.Text)

        ' Loop through and display each path.
        For Each path In list

            If InStr(path, ".php") Then
                Dim fileDetail As IO.FileInfo
                Dim CurFileName As String
                CurFileName = FileNameWithoutExtension(path)
                fileDetail = My.Computer.FileSystem.GetFileInfo(CurFileName)
                If fileDetail.Extension = ".php" Then
                    'Console.WriteLine(fileDetail.Extension)
                    txtfiles.Text = txtfiles.Text & path & vbCrLf
                End If
            Else
                'add later to support other file types
            End If
        Next

        ' Write total number of paths found.
        'Console.WriteLine(list.Count)
        ' Label5.Text = list.Count
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TxtFiles.Text = ""
        TXTPath.Text = ""
        TXTPHP.Text = ""
        RichTextBox1.Text = ""
        If ChkFolder.Checked = True Then

            FolderBrowserDialog1.ShowDialog()
            TXTPath.Text = FolderBrowserDialog1.SelectedPath
        Else
            With OpenFileDialog1
                .Title = "Select A File To Scan."
                .FileName = ""
                .ShowDialog()
            End With
            TXTPath.Text = OpenFileDialog1.FileName
        End If

        If TXTPath.Text = "" Then
            Exit Sub
        Else
            If ChkFolder.Checked = True Then
                Call ParseShitz()
                LabelFileCount.Text = TxtFiles.Lines.Count & " Files To Check"
                Try
                    Call LoadPHP(TxtFiles.Lines(0))
                Catch ex As Exception
                    MsgBox("There are no php files in selected folder!")
                    Exit Sub
                End Try
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = .ForeColor
                    If RichTextBox1.Text = "" Then
                        .AppendText("Successfully Loaded PHP!")
                    Else
                        .AppendText(Environment.NewLine & Environment.NewLine & "Successfully Loaded PHP!")
                    End If

                End With
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = .ForeColor
                    .AppendText(Environment.NewLine & Environment.NewLine & "Checking For Vulns!" & Environment.NewLine & TxtFiles.Lines(0))
                End With
                TxtFiles.Text = Replace(TxtFiles.Text, TxtFiles.Lines(0) & vbCrLf, "")
                LabelFileCount.Text = TxtFiles.Lines.Count & " Files To Check"
            Else
                Call LoadPHP(TXTPath.Text)
                With RichTextBox1
                    .SelectionStart = .TextLength
                    .SelectionColor = .ForeColor
                    .AppendText(Environment.NewLine & Environment.NewLine & "Checking For Vulns!" & Environment.NewLine & TXTPath.Text)
                End With
            End If

        End If
       
    End Sub
    Public Shared Function GetFilesRecursive(ByVal initial As String) As List(Of String)
        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(initial)

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string
            Dim dir As String = stack.Pop
            Try
                ' Add all immediate file paths
                result.AddRange(Directory.GetFiles(dir, "*.*"))

                ' Loop through all subdirectories and add them to the stack.
                Dim directoryName As String
                For Each directoryName In Directory.GetDirectories(dir)
                    stack.Push(directoryName)
                Next

            Catch ex As Exception
            End Try
        Loop

        ' Return the list
        Return result
    End Function

    Private Sub LoadPHP(ByVal PhpSrc As String)
        Me.Text = MyName & " Loading PHP: " & PhpSrc
        Application.DoEvents()
        TXTPHP.Text = FileIO.FileSystem.ReadAllText(PhpSrc)
        Me.Text = MyName
        Application.DoEvents()
    End Sub


    Private Sub RichTextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
       
    End Sub

    Private Sub RichTextBox1_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichTextBox1.TextChanged
        RichTextBox1.SelectionStart = RichTextBox1.TextLength
        RichTextBox1.ScrollToCaret()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyName = Me.Text
    End Sub
End Class
