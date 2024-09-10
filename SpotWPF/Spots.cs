using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using NntpBase.Nntp;
using NntpBase.Nntp.Models;
using NntpBase.Nntp.Responses;

namespace SpotWPF {
    internal class Spots : IList<SpotData>, IList {
        private const int cBlockSize = 100;

        private Data mData;
        private int mNumberSpots;
        private List<SpotData> mSpotsLow;
        private List<SpotData> mSpotsHigh;
        private int mCurrentBlock;

        internal Spots() {
            mData = Data.getInstance;
            sInitPaging();
            mNumberSpots = 0;   
        }

        #region IList<T>, IList

        #region Count
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// The first time this property is accessed, it will fetch the count from the IItemsProvider.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public virtual int Count {
            get {
                return mNumberSpots;
            }
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Gets the item at the specified index. This property will fetch
        /// the corresponding page from the IItemsProvider if required.
        /// </summary>
        /// <value></value>
        public SpotData this[int index] {
            get {
                return xGetSpot(index);
            }
            set { throw new NotSupportedException(); }
        }

        object IList.this[int index] {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region IEnumerator<T>, IEnumerator

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <remarks>
        /// This method should be avoided on large collections due to poor performance.
        /// </remarks>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<SpotData> GetEnumerator() {
            for (int i = 0; i < Count; i++) {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Add

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Add(SpotData pSpotData) {
            throw new NotSupportedException();
        }

        int IList.Add(object value) {
            throw new NotSupportedException();
        }

        #endregion

        #region Contains
        public bool Contains(SpotData pSpotData) {
            return false;
        }

        bool IList.Contains(object value) {
            return Contains((SpotData)value);
        }

        #endregion

        #region Clear
        public void Clear() {
            throw new NotSupportedException();
        }

        #endregion

        #region IndexOf
        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>
        /// Always -1.
        /// </returns>
        public int IndexOf(SpotData pSpotData) {
            return -1;
        }
        int IList.IndexOf(object value) {
            return IndexOf((SpotData)value);
        }

        #endregion

        #region Insert

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        /// </exception>
        public void Insert(int index, SpotData pSpotData) {
            throw new NotSupportedException();
        }

        void IList.Insert(int index, object value) {
            Insert(index, (SpotData)value);
        }
        #endregion

        #region Remove

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        /// </exception>
        public void RemoveAt(int index) {
            throw new NotSupportedException();
        }

        void IList.Remove(object value) {
            throw new NotSupportedException();
        }

        public bool Remove(SpotData pSpotData) {
            throw new NotSupportedException();
        }

        #endregion

        #region CopyTo
        public void CopyTo(SpotData[] array, int arrayIndex) {
            throw new NotSupportedException();
        }

        void ICollection.CopyTo(Array array, int index) {
            throw new NotSupportedException();
        }
        #endregion

        #region Misc

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        public object SyncRoot {
            get { return this; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>Always false.
        /// </returns>
        public bool IsSynchronized {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>Always true.
        /// </returns>
        public bool IsReadOnly {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <value></value>
        /// <returns>Always false.
        /// </returns>
        public bool IsFixedSize {
            get { return false; }
        }
        #endregion

        #endregion

        internal async Task xInitSpotsAsync() {
            sInitPaging();
            mNumberSpots = await mData.xGetNumberSpotsAsync().ConfigureAwait(false);
        }

        private void sInitPaging() {
            mCurrentBlock = -9;
            mSpotsLow = null;
            mSpotsHigh = null;
        }

        internal int xNumberSpots {
            get {
                return mNumberSpots;
            }
        }

        internal SpotData xGetSpot(int pLineNumber) {
            int lBlock;
            int lLine;
            List<SpotData> lSpots;

            lBlock = pLineNumber / cBlockSize;
            lLine = pLineNumber % cBlockSize;
            if (lBlock < mCurrentBlock || lBlock > mCurrentBlock + 1) {
                sGetSpotBlock(lBlock, lLine);
            }
            if (lBlock == mCurrentBlock) {
                lSpots = mSpotsLow;
            } else {
                lSpots = mSpotsHigh;
            }
            if (lSpots == null) {
                return default(SpotData);
            } else {
                return lSpots[lLine];
            }
        }

        private void sGetSpotBlock(int pBlock, int pLine) {
            int lStart;

            if (pBlock == mCurrentBlock + 2) {
                mSpotsLow = mSpotsHigh;
                mSpotsHigh = sGetBlock(pBlock);
                mCurrentBlock++;
            } else {
                if (pBlock == mCurrentBlock - 1) {
                    mSpotsHigh = mSpotsLow;
                    mSpotsLow = sGetBlock(pBlock);
                    mCurrentBlock--;
                } else {
                    if (pLine < 500 && pBlock > 0) {
                        lStart = pBlock - 1;
                    } else {
                        lStart = pBlock;
                    }
                    mSpotsLow = sGetBlock(lStart);
                    mSpotsHigh = sGetBlock(lStart + 1);
                    mCurrentBlock = lStart;
                }
            }
        }

        private List<SpotData> sGetBlock(int pBlock) {
            return mData.xGetSpotsBlock(pBlock * cBlockSize, cBlockSize);
        }

        internal void xRefresh(bool pUpdate) {
            NntpResponse lResponse;
            NntpGroupResponse lGroupResponse;
            NntpClient lClient = new NntpClient(new NntpConnection());
            long lLow;
            long lHigh;
            NntpArticleRange lArticleRange;
            XoverHeaderProcessor lProcessor;
            List<string> lDispose = null;

            if (lClient.Connect(Global.gServer.xReader, Global.gServer.xPort, Global.gServer.xSSL)) {
                if (lClient.Authenticate(Global.gServer.xUserId, Global.gServer.xPassWord)) {
                    lGroupResponse = lClient.Group(Global.cSpotGroup);
                    if (lGroupResponse.Success) {
                        if (Global.gServer.xHighSpotId < lGroupResponse.Group.LowWaterMark) {
                            lLow = lGroupResponse.Group.LowWaterMark;
                        } else {
                            lLow = Global.gServer.xHighSpotId + 1;
                        }
                        if (lLow <= lGroupResponse.Group.HighWaterMark) {
                            lHigh = lGroupResponse.Group.HighWaterMark;
                            lArticleRange = new NntpArticleRange(lLow, lHigh);
                            lProcessor = new XoverHeaderProcessor();
                            lResponse = lClient.Xover(lArticleRange, lProcessor);
                            if (lResponse.Success) {
                                lDispose = lProcessor.xDispose;
                                Global.gServer.xSetHighSpotId(lHigh, pUpdate);
                                Global.gServer.xLastRefresh = DateTime.Now;
                                mData.xUpdateServer(Global.gServer);
                            }
                            if (lDispose != null) {
                                foreach (var bDispose in lDispose) {
                                    mData.xDeleteSpot(bDispose);
                                }
                            }
                            sInitPaging();
                            mNumberSpots = mData.xGetNumberSpots();
                        }
                    }
                    lResponse = lClient.Quit();
                }
            }
        }

        internal void xGetSpotRaw(string pMessageId) {
            NntpResponse lResponse;
            NntpClient lClient = new NntpClient(new NntpConnection());

            if (lClient.Connect(Global.gServer.xReader, Global.gServer.xPort, Global.gServer.xSSL)) {
                if (lClient.Authenticate(Global.gServer.xUserId, Global.gServer.xPassWord)) {
                    lResponse = lClient.Article(new NntpMessageId(pMessageId), new RawProcessor());
                    lResponse = lClient.Quit();
                }
            }

        }
    }
}
