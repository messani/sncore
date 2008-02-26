Option Strict Off
Option Explicit Off
Imports System
Imports EnvDTE
Imports EnvDTE80
Imports System.Diagnostics

Public Module PuzzleMacros
    Private Sub Replace(ByVal s As String, ByVal t As String, ByVal f As String, ByVal stx As vsFindPatternSyntax)
        Replace(s, t, f, stx, "Entire Solution")
    End Sub

    Private Sub Replace(ByVal s As String, ByVal t As String, ByVal f As String, ByVal stx As vsFindPatternSyntax, ByVal searchPath As String)
        Try
            DTE.ExecuteCommand("Edit.FindinFiles")
            DTE.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Activate() 'Find and Replace
            DTE.ExecuteCommand("Edit.SwitchtoReplaceInFiles")
            DTE.Find.FilesOfType = f
            DTE.Find.Action = vsFindAction.vsFindActionReplaceAll
            DTE.Windows.Item(Constants.vsWindowKindSolutionExplorer).Activate()
            DTE.Find.FindWhat = s
            DTE.Find.ReplaceWith = t
            DTE.Find.Target = vsFindTarget.vsFindTargetFiles
            DTE.Find.MatchCase = False
            DTE.Find.MatchWholeWord = False
            DTE.Find.MatchInHiddenText = True
            DTE.Find.PatternSyntax = stx
            DTE.Find.SearchPath = searchPath
            DTE.Find.SearchSubfolders = True
            DTE.Find.KeepModifiedDocumentsOpen = False
            DTE.Find.ResultsLocation = vsFindResultsLocation.vsFindResults1
            DTE.Find.Action = vsFindAction.vsFindActionReplaceAll
            If (DTE.Find.Execute() = vsFindResult.vsFindResultNotFound) Then
                Throw New System.Exception("vsFindResultNotFound")
            End If
            DTE.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Close()
        Catch ex As Exception
            DTE.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Close()
        End Try
    End Sub

    Private Sub ReplaceProperty(ByVal name As String, ByVal column As String, ByVal type As String, ByVal targettype As String)
        Replace( _
            "<property name=""" + name + """ column=""" + column + """ type=""" + type + """ />", _
            "<property name=""" + name + """ column=""" + column + """ type=""" + targettype + """ />", _
            "*.hbm.xml", _
            vsFindPatternSyntax.vsFindPatternSyntaxLiteral)
    End Sub

    Public Sub ReplaceOM()
        AddFileToProject("SnCore.Data", "IDbObject.cs")
        AddFileToProject("SnCore.Data", "IDbPictureObject.cs")
        DTE.ActiveWindow.Object.GetItem("SnCore\SnCore.Data").Select(vsUISelectionType.vsUISelectionTypeSelect)
        Replace("(public class )(:a*)", "\0 : IDbObject", "*.cs", vsFindPatternSyntax.vsFindPatternSyntaxRegExpr, "Current Project")
        Replace("(public class )(:a+)(Picture)([:b\:]+)", "\0IDbPictureObject, ", "*.cs", vsFindPatternSyntax.vsFindPatternSyntaxRegExpr, "Current Project")
        Replace("public  ", "virtual public ", "*.cs", vsFindPatternSyntax.vsFindPatternSyntaxLiteral, "Current Project")
        Replace("<bag ", "<bag lazy=""true"" ", "*.hbm.xml", vsFindPatternSyntax.vsFindPatternSyntaxLiteral)
        Replace("Byte[]", "BinaryBlob", "*.hbm.xml", vsFindPatternSyntax.vsFindPatternSyntaxLiteral)
        Replace("urn:nhibernate-mapping-2.0", "urn:nhibernate-mapping-2.2", "*.hbm.xml", vsFindPatternSyntax.vsFindPatternSyntaxLiteral)
        Replace("\<\!--.*\n", "", "*.hbm.xml", vsFindPatternSyntax.vsFindPatternSyntaxRegExpr)
        ReplaceProperty("Summary", "Summary", "String", "StringClob")
        ReplaceProperty("Xsl", "Xsl", "String", "StringClob")
        ReplaceProperty("Body", "Body", "String", "StringClob")
        ReplaceProperty("Description", "Description", "String", "StringClob")
        ReplaceProperty("Message", "Message", "String", "StringClob")
        ReplaceProperty("Details", "Details", "String", "StringClob")
        Replace("[assembly: AssemblyKeyFile("""")]", "[assembly: AssemblyKeyFile(""..\\key.snk"")]", "AssemblyInfo.cs", vsFindPatternSyntax.vsFindPatternSyntaxLiteral)
    End Sub

    Private Sub AddFileToProject(ByVal projectname As String, ByVal filename As String)
        For Each project As EnvDTE.Project In DTE.Solution.Projects
            If project.Name = projectname Then
                project.DTE.ItemOperations.AddExistingItem(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(project.FileName), filename))
                Exit For
            End If
        Next
    End Sub
End Module
