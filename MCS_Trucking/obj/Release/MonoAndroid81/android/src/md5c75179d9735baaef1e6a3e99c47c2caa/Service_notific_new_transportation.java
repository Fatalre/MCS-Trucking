package md5c75179d9735baaef1e6a3e99c47c2caa;


public class Service_notific_new_transportation
	extends android.app.Service
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onBind:(Landroid/content/Intent;)Landroid/os/IBinder;:GetOnBind_Landroid_content_Intent_Handler\n" +
			"n_onCreate:()V:GetOnCreateHandler\n" +
			"";
		mono.android.Runtime.register ("MCS_Trucking.Service_notific_new_transportation, MCS_Trucking", Service_notific_new_transportation.class, __md_methods);
	}


	public Service_notific_new_transportation ()
	{
		super ();
		if (getClass () == Service_notific_new_transportation.class)
			mono.android.TypeManager.Activate ("MCS_Trucking.Service_notific_new_transportation, MCS_Trucking", "", this, new java.lang.Object[] {  });
	}


	public android.os.IBinder onBind (android.content.Intent p0)
	{
		return n_onBind (p0);
	}

	private native android.os.IBinder n_onBind (android.content.Intent p0);


	public void onCreate ()
	{
		n_onCreate ();
	}

	private native void n_onCreate ();

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
