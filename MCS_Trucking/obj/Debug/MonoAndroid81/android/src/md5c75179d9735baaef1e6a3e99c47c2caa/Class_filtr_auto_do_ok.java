package md5c75179d9735baaef1e6a3e99c47c2caa;


public class Class_filtr_auto_do_ok
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("MCS_Trucking.Class_filtr_auto_do_ok, MCS_Trucking", Class_filtr_auto_do_ok.class, __md_methods);
	}


	public Class_filtr_auto_do_ok ()
	{
		super ();
		if (getClass () == Class_filtr_auto_do_ok.class)
			mono.android.TypeManager.Activate ("MCS_Trucking.Class_filtr_auto_do_ok, MCS_Trucking", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
