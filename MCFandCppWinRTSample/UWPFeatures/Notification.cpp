#include "stdafx.h"
#include "Notification.h"

#include "winrt/Windows.UI.Notifications.h"
#include "winrt/Windows.Data.Xml.Dom.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Notifications;


extern "C" void ShowNotification(const wchar_t* text)
{
	// 今から 2 時間有効なトースト通知を出す
	auto notificationManager = ToastNotificationManager::GetDefault();
	auto toastXml = ToastNotificationManager::GetTemplateContent(ToastTemplateType::ToastText01);
	auto textNode = toastXml.GetElementsByTagName(L"text").Item(0);
	textNode.AppendChild(toastXml.CreateTextNode(text));
	auto toast = ToastNotification(toastXml);
	toast.ExpirationTime(winrt::clock::now() + std::chrono::hours() * 2);
	notificationManager.CreateToastNotifier().Show(toast);
}