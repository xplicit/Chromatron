/*
* chromatron_mac.m
*/

// include the Cocoa Frameworks
#import <Cocoa/Cocoa.h>
#import <objc/runtime.h>
#include "cef_application_mac.h"
#include "include/internal/cef_types_mac.h"

// include Chromatron custom header
#include "chromatron_mac.h"

namespace {
  bool g_handling_send_event = false;
}  // namespace

/*
* I1 - ChromatronApplication manages events.
* Provide the CefAppProtocol implementation required by CEF.
*/
@interface NSApplication (ChromatronApplication) <CefAppProtocol>
- (void)_swizzled_sendEvent:(NSEvent*)event;
- (void)_swizzled_terminate:(id)sender;

@end


// I2 - Receives notifications from controls and the browser window. Will delete
// itself when done.
@interface RootWindowDelegate : NSObject <NSWindowDelegate> {
 @private
  NSWindow* window_;
  CHROMATRONPARAM chromatronParam_;
  bool force_close_;
}

@property(nonatomic, readwrite) bool force_close;

- (id)initWithWindow:(NSWindow*)window
       andParams:(CHROMATRONPARAM)param;
@end  // @interface RootWindowDelegate


/*
* I3 - ChromatronAppDelegate manages events.
*/
@interface ChromatronAppDelegate : NSObject <NSApplicationDelegate>  {
    NSWindow * window_;
    RootWindowDelegate* window_delegate_;
    NSView   *cefParentView_;
    CHROMATRONPARAM chromatronParam_;
}

- (void)setParams:(CHROMATRONPARAM)param;
- (void)createApplication:(id)object;
- (NSUInteger)windowCustomStyle;

- (void)tryToTerminateApplication:(NSApplication*)app;
@end

/*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/

@implementation RootWindowDelegate

@synthesize force_close = force_close_;

- (id)initWithWindow:(NSWindow*)window
        andParams:(CHROMATRONPARAM)param {
  if (self = [super init]) {
    window_ = window;
    chromatronParam_ = param;
    [window_ setDelegate:self];
    force_close_ = false;

    // Register for application hide/unhide notifications.
    [[NSNotificationCenter defaultCenter]
        addObserver:self
           selector:@selector(applicationDidHide:)
               name:NSApplicationDidHideNotification
             object:nil];
    [[NSNotificationCenter defaultCenter]
        addObserver:self
           selector:@selector(applicationDidUnhide:)
               name:NSApplicationDidUnhideNotification
             object:nil];
  }
  return self;
}

- (void)dealloc {
  [[NSNotificationCenter defaultCenter] removeObserver:self];
#if !__has_feature(objc_arc)
  [super dealloc];
#endif  // !__has_feature(objc_arc)
}

// Called when we are activated (when we gain focus).
- (void)windowDidBecomeKey:(NSNotification*)notification {
    /// CHROMATRONPARAM- function pointer?
}

// Called when we are deactivated (when we lose focus).
- (void)windowDidResignKey:(NSNotification*)notification {
    /// CHROMATRONPARAM- function pointer?
}

// Called when we have been minimized.
- (void)windowDidMiniaturize:(NSNotification*)notification {
    /// CHROMATRONPARAM- function pointer?
}

// Called when we have been unminimized.
- (void)windowDidDeminiaturize:(NSNotification*)notification {
    /// CHROMATRONPARAM- function pointer?
}

// Called when the application has been hidden.
- (void)applicationDidHide:(NSNotification*)notification {
  // If the window is miniaturized then nothing has really changed.
  if (![window_ isMiniaturized]) {
    /// CHROMATRONPARAM- function pointer?
  }
}

// Called when the application has been unhidden.
- (void)applicationDidUnhide:(NSNotification*)notification {
  // If the window is miniaturized then nothing has really changed.
  if (![window_ isMiniaturized]) {
    /// CHROMATRONPARAM- function pointer?
  }
}

// Called when the window is about to close. Perform the self-destruction
// sequence by getting rid of the window. By returning YES, we allow the window
// to be removed from the screen.
- (BOOL)windowShouldClose:(NSWindow*)window {
  if (!force_close_) {
        /// CHROMATRONPARAM- function pointer?
    }

  // Don't want any more delegate callbacks after we destroy ourselves.
  window_.delegate = nil;
  // Delete the window.
#if !__has_feature(objc_arc)
  [window autorelease];
#endif  // !__has_feature(objc_arc)
  window_ = nil;

  // Clean ourselves up after clearing the stack of anything that might have the
  // window on it.
  [self cleanup];

  // Allow the close.
  return YES;
}

// Deletes itself.
- (void)cleanup {
  // Don't want any more delegate callbacks after we destroy ourselves.
  window_.delegate = nil;
  window_.contentView = [[NSView alloc] initWithFrame:NSZeroRect];
  // Delete the window.
#if !__has_feature(objc_arc)
  [window_ autorelease];
#endif  // !__has_feature(objc_arc)
  window_ = nil;
 /// root_window_->OnNativeWindowClosed(); -     /// CHROMATRONPARAM- function pointer?

  chromatronParam_.exitCallback();
}

@end  // @implementation RootWindowDelegate

/*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/


/*
* ChromatronApplication manages events.
* Provide the CefAppProtocol implementation required by CEF.
*/

@implementation NSApplication (ChromatronApplication)

// This selector is called very early during the application initialization.
+ (void)load {
  // Swap NSApplication::sendEvent with _swizzled_sendEvent.
  Method original = class_getInstanceMethod(self, @selector(sendEvent));
  Method swizzled =
      class_getInstanceMethod(self, @selector(_swizzled_sendEvent));
  method_exchangeImplementations(original, swizzled);

  Method originalTerm = class_getInstanceMethod(self, @selector(terminate:));
  Method swizzledTerm =
      class_getInstanceMethod(self, @selector(_swizzled_terminate:));
  method_exchangeImplementations(originalTerm, swizzledTerm);
}

- (BOOL)isHandlingSendEvent {
   return g_handling_send_event;
}

- (void)setHandlingSendEvent:(BOOL)handlingSendEvent {
  g_handling_send_event = handlingSendEvent;
}

- (void)_swizzled_sendEvent:(NSEvent*)event {
  CefScopedSendingEvent sendingEventScoper;
  // Calls NSApplication::sendEvent due to the swizzling.
  [self _swizzled_sendEvent:event];
}

- (void)_swizzled_terminate:(id)sender {
  [self _swizzled_terminate:sender];
}

@end

/*
* ChromatronAppDelegate manages events.
*/

@implementation ChromatronAppDelegate

- (void)setParams:(CHROMATRONPARAM)param {
    chromatronParam_ = param;
}

// Create the application on the UI thread.
- (void)createApplication:(id)object {

  // Set the delegate for application events.
  NSApplication* application = [NSApplication sharedApplication];
  [application setDelegate:self];

  [application setActivationPolicy:NSApplicationActivationPolicyRegular];

  // Create the main window.

  // The CEF framework library is loaded at runtime so we need to use this
  // mechanism for retrieving the class.
  Class window_class = NSClassFromString(@"UnderlayOpenGLHostingWindow");

  NSUInteger customStyleMask = [self windowCustomStyle];
  window_ = [[window_class alloc]
               initWithContentRect: NSMakeRect(chromatronParam_.x, chromatronParam_.y, chromatronParam_.width, chromatronParam_.height)
               styleMask:customStyleMask
               backing:NSBackingStoreBuffered
               defer:NO ];

    NSString *title = [NSString stringWithUTF8String: chromatronParam_.titleUtf8Ptr];
	  [window_ setTitle:title];
    [window_ setAcceptsMouseMovedEvents:YES];

    // No dark mode, please
    window_.appearance = [NSAppearance appearanceNamed:NSAppearanceNameAqua];

    // Create the delegate for browser window events.
    window_delegate_ = [[RootWindowDelegate alloc] initWithWindow:window_
                                                  andParams:chromatronParam_];

    cefParentView_ = [window_ contentView];
    chromatronParam_.createCallback(window_, cefParentView_);

    if (chromatronParam_.centerscreen == 1)
        [window_ center];

    if (chromatronParam_.nominbutton == 1)
        [[window_ standardWindowButton:NSWindowMiniaturizeButton] setHidden:YES];

    if (chromatronParam_.nomaxbutton == 1)
        [[window_ standardWindowButton:NSWindowZoomButton] setHidden:YES];

    // Make window frameless without buttons and draggable invisible titlebar
    if (chromatronParam_.frameless == 1) {
        [[window_ standardWindowButton:NSWindowCloseButton] setHidden:YES];
        [[window_ standardWindowButton:NSWindowMiniaturizeButton] setHidden:YES];
        [[window_ standardWindowButton:NSWindowZoomButton] setHidden:YES];
        window_.titleVisibility = NSWindowTitleHidden;
        window_.titlebarAppearsTransparent = true;
    }

    [window_ makeKeyAndOrderFront:NSApp];

    // Rely on the window delegate to clean us up rather than immediately
    // releasing when the window gets closed. We use the delegate to do
    // everything from the autorelease pool so the window isn't on the stack
    // during cleanup (ie, a window close from javascript).
    [window_ setReleasedWhenClosed:NO];

    [NSApp activateIgnoringOtherApps:YES];
}

- (NSUInteger)windowCustomStyle {
    if (chromatronParam_.fullscreen == 1)
        return NSWindowStyleMaskFullScreen;

    NSUInteger styleMask = NSWindowStyleMaskTitled
                           | NSWindowStyleMaskClosable
                           | NSWindowStyleMaskMiniaturizable;

    if (chromatronParam_.frameless == 1)
        styleMask |= NSWindowStyleMaskFullSizeContentView;

    if (chromatronParam_.noresize == 0)
        styleMask |= NSWindowStyleMaskResizable;

    return styleMask;
}

- (void) windowWillMove:(NSNotification *)notification {
    chromatronParam_.movingCallback();
}

- (void)windowDidResize:(NSNotification *)notification {
    NSRect windowRect = [window_ frame];
    NSRect contentRect = [window_ contentRectForFrameRect:windowRect];
    chromatronParam_.resizeCallback(contentRect.size.width, contentRect.size.height);
}

- (void)tryToTerminateApplication:(NSApplication*)app {
    chromatronParam_.exitCallback();
}

- (NSApplicationTerminateReply)applicationShouldTerminate:
    (NSApplication*)sender {
  return NSTerminateNow;
}

- (void)dealloc {
    [cefParentView_ release];

    // Delete the window.
    #if !__has_feature(objc_arc)
      [window_ autorelease];
    #endif  // !__has_feature(objc_arc)
      window_ = nil;

    [super dealloc];

    chromatronParam_.exitCallback();
}

@end


/*
* Exported methods implementation
*/

void createwindow(CHROMATRONPARAM* pParam) {

      @autoreleasepool {
        // Initialize the ChromatronApplication instance.
        [NSApplication sharedApplication];

        // Create the application delegate.
        ChromatronAppDelegate *appDelegate = [[ChromatronAppDelegate alloc] init];
        [appDelegate setParams:*pParam];
        [appDelegate performSelectorOnMainThread:@selector(createApplication:)
                               withObject:nil
                               waitUntilDone:NO];

        // Run the CEF message loop. This will block until CefQuitMessageLoop() is
        // called.
        pParam->runMessageLoopCallback();

        // Shut down CEF.
        pParam->cefShutdownCallback();

        // Release the delegate.
        #if !__has_feature(objc_arc)
        [appDelegate release];
        #endif  // !__has_feature(objc_arc)
        appDelegate = nil;
    }  // @autoreleasepool
}

APPDATA createwindowdata(CHROMATRONPARAM* pParam) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    NSApp = [NSApplication sharedApplication];


    // Create the application delegate.
    ChromatronAppDelegate *appDelegate = [[[ChromatronAppDelegate alloc] init] autorelease];
    NSLog(@"Created: appDelegate appDelegate:%@", appDelegate);

    [appDelegate setParams:*pParam];
    NSLog(@"appDelegate setParams set");

    [appDelegate performSelectorOnMainThread:@selector(createApplication:)
                            withObject:nil
                            waitUntilDone:NO];
    NSLog(@"appDelegate performSelectorOnMainThread");

    APPDATA appData;
    appData.app = NSApp;
    appData.pool = pool;

    return appData;
}

void run(void* application) {
    [(NSApplication *)application run];
}

void quit(void* application, void* pool) {
    [(NSAutoreleasePool *)pool release];
    [(NSApplication *)application performSelector:@selector(terminate:) withObject:nil afterDelay:0.0];
}

void minimize(void* view) {
  [[CAST_CEF_WINDOW_HANDLE_TO_NSVIEW(view) window] performMiniaturize:nil];
}

void maximize(void* view) {
  [[CAST_CEF_WINDOW_HANDLE_TO_NSVIEW(view) window] performZoom:nil];
}
