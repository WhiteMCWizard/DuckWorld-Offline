namespace CinemaDirector.Helpers;

internal interface IRevertable
{
	RevertMode EditorRevertMode { get; set; }

	RevertMode RuntimeRevertMode { get; set; }

	RevertInfo[] CacheState();
}
