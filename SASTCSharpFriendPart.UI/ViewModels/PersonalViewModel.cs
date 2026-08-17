using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using SASTCSharpFriendPart_UI.Services;

namespace SASTCSharpFriendPart_UI.ViewModels;

public partial class PersonalViewModel : ObservableObject
{
    private readonly PersonalStore _personalStore;
    private string _savedName = string.Empty;
    private string _savedDescription = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private bool isEditing;

    public string ErrorMessage => _personalStore.ErrorMessage;

    public bool IsNotEditing => !IsEditing;

    public Visibility EditButtonVisible =>
        IsEditing ? Visibility.Collapsed : Visibility.Visible;

    public Visibility SaveButtonVisible =>
        IsEditing ? Visibility.Visible : Visibility.Collapsed;

    public Visibility CancelButtonVisible => SaveButtonVisible;

    public PersonalViewModel()
        : this(App.PersonalStore)
    {
    }

    public PersonalViewModel(PersonalStore personalStore)
    {
        _personalStore = personalStore;
        _personalStore.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(PersonalStore.ErrorMessage))
            {
                OnPropertyChanged(nameof(ErrorMessage));
            }
        };

        Name = _personalStore.Name;
        Description = _personalStore.Description;
        RememberSavedValues();
    }

    partial void OnIsEditingChanged(bool value)
    {
        OnPropertyChanged(nameof(IsNotEditing));
        OnPropertyChanged(nameof(EditButtonVisible));
        OnPropertyChanged(nameof(SaveButtonVisible));
        OnPropertyChanged(nameof(CancelButtonVisible));
    }

    [RelayCommand]
    private void Edit()
    {
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            _personalStore.ErrorMessage = "名称不能为空。";
            OnPropertyChanged(nameof(ErrorMessage));
            return;
        }

        Name = Name.Trim();
        _personalStore.Name = Name;
        _personalStore.Description = Description.Trim();

        if (_personalStore.Save())
        {
            Description = _personalStore.Description;
            RememberSavedValues();
            IsEditing = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        Name = _savedName;
        Description = _savedDescription;
        _personalStore.ErrorMessage = string.Empty;
        OnPropertyChanged(nameof(ErrorMessage));
        IsEditing = false;
    }

    private void RememberSavedValues()
    {
        _savedName = Name;
        _savedDescription = Description;
    }
}
