package md5c75179d9735baaef1e6a3e99c47c2caa;


public class Class_prosmotr_month_calendar
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"";
		mono.android.Runtime.register ("MCS_Trucking.Class_prosmotr_month_calendar, MCS_Trucking", Class_prosmotr_month_calendar.class, __md_methods);
	}


	public Class_prosmotr_month_calendar ()
	{
		super ();
		if (getClass () == Class_prosmotr_month_calendar.class)
			mono.android.TypeManager.Activate ("MCS_Trucking.Class_prosmotr_month_calendar, MCS_Trucking", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
