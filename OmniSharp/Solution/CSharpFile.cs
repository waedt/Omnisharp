using System;
using System.IO;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;

namespace OmniSharp.Solution
{
    public class CSharpFile
    {
        public string FileName;

        public ITextSource Content;
        public SyntaxTree SyntaxTree;
        public IUnresolvedFile ParsedFile;


        public ReadOnlyDocument Document { get; set; }

        public CSharpFile(IProject project, string fileName) : this(project, fileName, File.ReadAllText(fileName))
        {
        }

        public CSharpFile(IProject project, string fileName, string source)
        {
            Parse(project, fileName, source);
        }

        private void Parse(IProject project, string fileName, string source)
        {
            Console.WriteLine("Loading " + fileName);
            this.FileName = fileName;
            this.Content = new StringTextSource(source);
            this.Document = new ReadOnlyDocument(this.Content);
            this.Project = project;
            CSharpParser p = project.CreateParser();
            this.SyntaxTree = p.Parse(Content.CreateReader(), fileName);
            if (p.HasErrors)
            {
                Console.WriteLine("Error parsing " + fileName + ":");
                foreach (var error in p.Errors)
                {
                    Console.WriteLine("  " + error.Region + " " + error.Message);
                }
            }
            this.ParsedFile = this.SyntaxTree.ToTypeSystem();
        }

        protected IProject Project { get; set; }

        public void Update(string source)
        {
            Parse(Project, this.FileName, source);
        }
    }
}