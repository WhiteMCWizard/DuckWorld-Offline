namespace CinemaDirector;

internal interface IOptimizable
{
	bool CanOptimize { get; set; }

	void Optimize();
}
