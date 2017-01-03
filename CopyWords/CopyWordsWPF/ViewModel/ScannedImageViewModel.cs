using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CopyWordsWPF.ViewModel.Commands;

namespace CopyWordsWPF.ViewModel
{
    public class ScannedImageViewModel : BindableBase
    {
        private const string IsPreviousPageAvaliableeProperty = "IsPreviousPageAvaliable";
        private const string IsNextPageAvaliableProperty = "IsNextPageAvaliable";
        private const string CurrentPageImageProperty = "CurrentPageImage";
        private const string TitleProperty = "Title";

        private bool _isPreviousPageAvailable;
        private bool _isNextPageAvaliable;
        private string _currentPage;
        private string _title;        
        private BitmapImage _currentPageImage;
        private List<string> _allPages;

        private ChangePageCommand _changePageCommand;

        public ScannedImageViewModel(string title, List<string> allPages, string currentPage)
        {
            Title = title;
            _currentPage = currentPage;
            _allPages = allPages;

            _changePageCommand = new ChangePageCommand(MoveForward, MoveBack);

            SetCurrentImage();
        }

        #region Properties

        public ChangePageCommand ChangePageCommand
        {
            get { return _changePageCommand; }
        }

        public bool IsPreviousPageAvaliable
        {
            get { return _isPreviousPageAvailable; }
            set
            {
                SetProperty<bool>(ref _isPreviousPageAvailable, value);
            }
        }

        public bool IsNextPageAvaliable
        {
            get { return _isNextPageAvaliable; }
            set
            {
                SetProperty<bool>(ref _isNextPageAvaliable, value);
            }
        }

        public BitmapImage CurrentPageImage
        {
            get { return _currentPageImage; }
            set
            {
                SetProperty<BitmapImage>(ref _currentPageImage, value);
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty<string>(ref _title, value);
            }
        } 

        #endregion

        #region Public Methods

        private string GetImageFilePath(string imageName)
        {
            string filePath = Path.Combine(CopyWordsWPF.Properties.Settings.Default.DRDictPath, imageName);
            return filePath;
        }
        
        private void SetCurrentImage()
        {
            var bitmap = new BitmapImage(new Uri(GetImageFilePath(_currentPage)));
            CurrentPageImage = bitmap;

            int currPageIndex = _allPages.IndexOf(_currentPage);
            IsPreviousPageAvaliable = currPageIndex > 0;
            IsNextPageAvaliable = currPageIndex < (_allPages.Count - 1);
        }
        
        private void MoveForward()
        {
            int currPageIndex = _allPages.IndexOf(_currentPage);
            _currentPage = _allPages[currPageIndex + 1];

            SetCurrentImage();
        }

        private void MoveBack()
        {
            int currPageIndex = _allPages.IndexOf(_currentPage);
            _currentPage = _allPages[currPageIndex - 1];

            SetCurrentImage();
        }

        #endregion        
    }
}
