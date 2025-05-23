#if UNITY_EDITOR || FBXSDK_RUNTIME
//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Autodesk.Fbx {

public class FbxCamera : FbxNodeAttribute {
  internal FbxCamera(global::System.IntPtr cPtr, bool ignored) : base(cPtr, ignored) { }

  // override void Dispose() {base.Dispose();}

  public new static FbxCamera Create(FbxManager pManager, string pName) {
    global::System.IntPtr cPtr = NativeMethods.FbxCamera_Create__SWIG_0(FbxManager.getCPtr(pManager), pName);
    FbxCamera ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxCamera(cPtr, false);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public new static FbxCamera Create(FbxObject pContainer, string pName) {
    global::System.IntPtr cPtr = NativeMethods.FbxCamera_Create__SWIG_1(FbxObject.getCPtr(pContainer), pName);
    FbxCamera ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxCamera(cPtr, false);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public void SetAspect(FbxCamera.EAspectRatioMode pRatioMode, double pWidth, double pHeight) {
    NativeMethods.FbxCamera_SetAspect(swigCPtr, (int)pRatioMode, pWidth, pHeight);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
  }

  public FbxCamera.EAspectRatioMode GetAspectRatioMode() {
    FbxCamera.EAspectRatioMode ret = (FbxCamera.EAspectRatioMode)NativeMethods.FbxCamera_GetAspectRatioMode(swigCPtr);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public void SetNearPlane(double pDistance) {
    NativeMethods.FbxCamera_SetNearPlane(swigCPtr, pDistance);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
  }

  public double GetNearPlane() {
    double ret = NativeMethods.FbxCamera_GetNearPlane(swigCPtr);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public void SetFarPlane(double pDistance) {
    NativeMethods.FbxCamera_SetFarPlane(swigCPtr, pDistance);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
  }

  public double GetFarPlane() {
    double ret = NativeMethods.FbxCamera_GetFarPlane(swigCPtr);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public void SetApertureMode(FbxCamera.EApertureMode pMode) {
    NativeMethods.FbxCamera_SetApertureMode(swigCPtr, (int)pMode);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
  }

  public FbxCamera.EApertureMode GetApertureMode() {
    FbxCamera.EApertureMode ret = (FbxCamera.EApertureMode)NativeMethods.FbxCamera_GetApertureMode(swigCPtr);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public void SetApertureWidth(double pWidth) {
    NativeMethods.FbxCamera_SetApertureWidth(swigCPtr, pWidth);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
  }

  public double GetApertureWidth() {
    double ret = NativeMethods.FbxCamera_GetApertureWidth(swigCPtr);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public void SetApertureHeight(double pHeight) {
    NativeMethods.FbxCamera_SetApertureHeight(swigCPtr, pHeight);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
  }

  public double GetApertureHeight() {
    double ret = NativeMethods.FbxCamera_GetApertureHeight(swigCPtr);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public double ComputeFocalLength(double pAngleOfView) {
    double ret = NativeMethods.FbxCamera_ComputeFocalLength(swigCPtr, pAngleOfView);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public FbxPropertyDouble AspectWidth {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxCamera_AspectWidth_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble AspectHeight {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxCamera_AspectHeight_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyEGateFit GateFit {
    get {
      FbxPropertyEGateFit ret = new FbxPropertyEGateFit(NativeMethods.FbxCamera_GateFit_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble FieldOfView {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxCamera_FieldOfView_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble FocalLength {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxCamera_FocalLength_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble NearPlane {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxCamera_NearPlane_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble FilmAspectRatio {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxCamera_FilmAspectRatio_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble FilmOffsetX {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxCamera_FilmOffsetX_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble FilmOffsetY {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxCamera_FilmOffsetY_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyEProjectionType ProjectionType {
    get {
      FbxPropertyEProjectionType ret = new FbxPropertyEProjectionType(NativeMethods.FbxCamera_ProjectionType_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyBool UseDepthOfField {
    get {
      FbxPropertyBool ret = new FbxPropertyBool(NativeMethods.FbxCamera_UseDepthOfField_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble FocusDistance {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxCamera_FocusDistance_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public override int GetHashCode(){
      return swigCPtr.Handle.GetHashCode();
  }

  public bool Equals(FbxCamera other) {
    if (object.ReferenceEquals(other, null)) { return false; }
    return this.swigCPtr.Handle.Equals (other.swigCPtr.Handle);
  }

  public override bool Equals(object obj){
    if (object.ReferenceEquals(obj, null)) { return false; }
    /* is obj a subclass of this type; if so use our Equals */
    var typed = obj as FbxCamera;
    if (!object.ReferenceEquals(typed, null)) {
      return this.Equals(typed);
    }
    /* are we a subclass of the other type; if so use their Equals */
    if (typeof(FbxCamera).IsSubclassOf(obj.GetType())) {
      return obj.Equals(this);
    }
    /* types are unrelated; can't be a match */
    return false;
  }

  public static bool operator == (FbxCamera a, FbxCamera b) {
    if (object.ReferenceEquals(a, b)) { return true; }
    if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) { return false; }
    return a.Equals(b);
  }

  public static bool operator != (FbxCamera a, FbxCamera b) {
    return !(a == b);
  }

  public enum EProjectionType {
    ePerspective,
    eOrthogonal
  }

  public enum EAspectRatioMode {
    eWindowSize,
    eFixedRatio,
    eFixedResolution,
    eFixedWidth,
    eFixedHeight
  }

  public enum EApertureMode {
    eHorizAndVert,
    eHorizontal,
    eVertical,
    eFocalLength
  }

  public enum EGateFit {
    eFitNone,
    eFitVertical,
    eFitHorizontal,
    eFitFill,
    eFitOverscan,
    eFitStretch
  }

}

}

#endif // UNITY_EDITOR || FBXSDK_RUNTIME