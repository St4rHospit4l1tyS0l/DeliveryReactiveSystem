using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Drs.Model.Settings;
using Drs.Model.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public class PagerVm : UcViewModelBase, IPagerVm
    {
        private int _page;
        private int _pages;
        private string _pagerPos;
        private readonly PagerModel _pagerModel = new PagerModel();
        private int _total;
        private int _size;
        private string _totalFound;


        public IReactiveCommand<Unit> CmdBackAll { get; set; }
        public IReactiveCommand<Unit> CmdBack { get; set; }
        public IReactiveCommand<Unit> CmdForward { get; set; }
        public IReactiveCommand<Unit> CmdForwardAll { get; set; }

        public event Action PagerChanged;

        public PagerVm()
        {
            Size = SettingsData.Client.RowsSizeGrids;
            var canExec = this.WhenAnyValue(vm => vm.Page, p => p > 0);
            CmdBackAll = ReactiveCommand.CreateAsyncTask(canExec, _ => Task.Run(() => DoBackAll()));
            CmdBack = ReactiveCommand.CreateAsyncTask(canExec, _ => Task.Run(() => DoBack()));
            canExec = this.WhenAnyValue(vm => vm.Pages, vm => vm.Page, (ps, p) => p < (ps - 1));
            CmdForward = ReactiveCommand.CreateAsyncTask(canExec, _ => Task.Run(() => DoForward()));
            CmdForwardAll = ReactiveCommand.CreateAsyncTask(canExec, _ => Task.Run(() => DoForwardAll()));
        }


        public int Page
        {
            get { return _page; }
            set
            {
                this.RaiseAndSetIfChanged(ref _page, value);
            }
        }

        public void Reset()
        {
            _pagerModel.Page = 0;
            _pagerModel.Total = 0;
            //Page = 0;
            //Pages = 0;
            //Total = 0;
            //SetPagerPos();            
        }

        public PagerModel PagerModel
        {
            get
            {
               
                return _pagerModel;
            }
            set
            {
                _pagerModel.ExtraData = value.ExtraData;
                _pagerModel.Page = value.Page;
                _pagerModel.Total = value.Total;
            }
        }

        public int Size
        {
            get { return _size; }
            set
            {
                this.RaiseAndSetIfChanged(ref _size, value);
            }
        }

        public void SetPager(PagerModel pager)
        {
            PagerModel = pager;
            Page = pager.Page;
            Pages = pager.CalculatePages();
            Total = pager.Total;
            SetPagerPos();
        }

        public int Total
        {
            get { return _total; }
            set
            {
                this.RaiseAndSetIfChanged(ref _total, value);
            }
        }

        private void SetPagerPos()
        {
            var rightLim = ((Page + 1) * Size);
            if (rightLim > Total)
                rightLim = Total;
            var leftLim = (Page * Size) + 1;
            if (Total == 0)
                leftLim = 0;

            PagerPos = String.Format("{0} / {1}", Total == 0 ? 0 : (Page + 1), Pages);
            TotalFound = String.Format("Mostrando del {0} al {1} de {2} registro(s)", leftLim, rightLim, Total);
            TotalFound = GetExtraInfo(TotalFound, PagerModel);
        }

        private string GetExtraInfo(string totalFound, PagerModel pagerModel)
        {
            if (pagerModel.ExtraData == null)
                return totalFound;

            decimal totalOrder;

            if (!decimal.TryParse(pagerModel.ExtraData.ToString(), out totalOrder))
                return totalFound;

            return String.Format("{0}. Total de las órdenes consultadas: ${1:#,##0.00}", totalFound, pagerModel.ExtraData);
        }

        public int Pages
        {
            get { return _pages; }
            set
            {
                this.RaiseAndSetIfChanged(ref _pages, value);
            }
        }

        public string PagerPos
        {
            get { return _pagerPos; }
            set
            {
                this.RaiseAndSetIfChanged(ref _pagerPos, value);
            }
        }

        private void DoBackAll()
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                _pagerModel.Page = 0;
                OnPagerChanged();
            });
        }

        private void DoBack()
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                try
                {
                    _pagerModel.Page = (_pagerModel.Page == 0) ? 0 : _pagerModel.Page - 1;
                    OnPagerChanged();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                } 
            });
        }

        private void DoForward()
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                try
                {
                    var pages = _pagerModel.CalculatePages();
                    _pagerModel.Page = (_pagerModel.Page == pages - 1) ? pages - 1 : _pagerModel.Page + 1;
                    OnPagerChanged();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        private void DoForwardAll()
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                _pagerModel.Page = _pagerModel.CalculatePages() - 1;
                OnPagerChanged();
            });
        }

        public string TotalFound
        {
            get { return _totalFound; }
            set
            {
                this.RaiseAndSetIfChanged(ref _totalFound, value);
            }
        }
        protected virtual void OnPagerChanged()
        {
            var handler = PagerChanged;
            if (handler != null) handler();
        }

    }
}