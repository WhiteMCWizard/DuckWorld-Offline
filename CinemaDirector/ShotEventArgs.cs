using System;

namespace CinemaDirector;

public class ShotEventArgs : EventArgs
{
	public CinemaShot shot;

	public ShotEventArgs(CinemaShot shot)
	{
		this.shot = shot;
	}
}
