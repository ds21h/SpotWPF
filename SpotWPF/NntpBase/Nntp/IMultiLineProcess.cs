namespace NntpBase.Nntp {
    public interface IMultiLineProcess {
        public void xStartProcess();
        public bool xProcessLine(string pLine);
        public void xEndProcess();
    }
}
