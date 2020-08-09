using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FFImageLoading;
using Mobile.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.IO;

namespace Mobile.ViewModels
{
    public class ItemPicturePageViewModel : ViewModelBase
    {
        public ItemPicturePageViewModel(InitParams initParams) : base(initParams)
        {
            Title = "Chỉnh sửa hình ảnh mặt hàng";
        }

        #region ImagePathBindProp
        private string _ImagePathBindProp = string.Empty;
        public string ImagePathBindProp
        {
            get { return _ImagePathBindProp; }
            set { SetProperty(ref _ImagePathBindProp, value); }
        }
        #endregion

        #region ImageBindProp
        private byte[] _ImageBindProp;
        public byte[] ImageBindProp
        {
            get { return _ImageBindProp; }
            set { SetProperty(ref _ImageBindProp, value); }
        }
        #endregion

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private async void OnSave(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(ImagePathBindProp))
            {
                await PageDialogService.DisplayAlertAsync("Lỗi", "Bạn chưa chọn ảnh.", "Đóng");
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day

                var param = new NavigationParameters();
                param.Add("ImagePath", ImagePathBindProp);
                param.Add("Image", ImageBindProp);
                await NavigationService.GoBackAsync(param);

            }
            catch (Exception e)
            {
                await ShowErrorAsync(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitSaveCommand()
        {
            SaveCommand = new DelegateCommand<object>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region TakeImageCommand

        public DelegateCommand<object> TakeImageCommand { get; private set; }
        private async void OnTakeImage(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await PageDialogService.DisplayAlertAsync("Lỗi", "Điện thoại không hổ trợ máy ảnh", "Đóng");
                return;
            }

            IsBusy = true;

            var fileName = $"{Guid.NewGuid()}.jpg";
            var filePath = "";
            try
            {
                // Thuc hien cong viec tai day
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                    storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (storageStatus == PermissionStatus.Granted && cameraStatus == PermissionStatus.Granted)
                {
                    try
                    {

                        var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            Name = fileName,
                            PhotoSize = PhotoSize.Full,
                            CompressionQuality = 92,
                            DefaultCamera = CameraDevice.Front,
                            SaveToAlbum = true,
                        });
                        if (file != null)
                        {
                            ImageBindProp = file.GetStream().ToByteArray();
                            filePath = file.Path;
                        }
                    }
                    catch (Exception ex)
                    {
                        await ShowErrorAsync(ex);
                    }
                }
                else if (storageStatus != PermissionStatus.Unknown || cameraStatus != PermissionStatus.Unknown)
                {
                    await PageDialogService.DisplayAlertAsync("Yêu cầu bị từ chối", "Không thể chụp ảnh.", "Đóng");
                }

                if (!string.IsNullOrEmpty(filePath))
                {
                    ImagePathBindProp = filePath;
                }
                else
                {
                    ImagePathBindProp = "";
                    ImageBindProp = null;
                }
            }
            catch (Exception e)
            {
                await ShowErrorAsync(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitTakeImageCommand()
        {
            TakeImageCommand = new DelegateCommand<object>(OnTakeImage);
            TakeImageCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectImageCommand

        public DelegateCommand<object> SelectImageCommand { get; private set; }
        private async void OnSelectImage(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await PageDialogService.DisplayAlertAsync("Lỗi", "Điện thoại không hổ trợ máy ảnh", "Đóng");
                return;
            }

            var filePath = "";

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                    storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (storageStatus == PermissionStatus.Granted && cameraStatus == PermissionStatus.Granted)
                {
                    try
                    {

                        var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                        {

                            PhotoSize = PhotoSize.Full,
                            CompressionQuality = 92
                        });

                        if (file != null)
                        {
                            ImageBindProp = file.GetStream().ToByteArray();
                            filePath = file.Path;
                        }
                    }
                    catch (Exception ex)
                    {
                        await ShowErrorAsync(ex);
                    }
                }
                else if (storageStatus != PermissionStatus.Unknown || cameraStatus != PermissionStatus.Unknown)
                {
                    await PageDialogService.DisplayAlertAsync("Yêu cầu bị từ chối", "Không thể chụp ảnh.", "Đóng");
                }

                if (string.IsNullOrEmpty(filePath) == false)
                {
                    ImagePathBindProp = filePath;
                }
                else
                {
                    ImagePathBindProp = "";
                    ImageBindProp = null;
                }
            }
            catch (Exception e)
            {
                await ShowErrorAsync(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitSelectImageCommand()
        {
            SelectImageCommand = new DelegateCommand<object>(OnSelectImage);
            SelectImageCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

    }
}
