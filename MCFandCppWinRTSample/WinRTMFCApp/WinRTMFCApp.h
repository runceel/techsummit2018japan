
// WinRTMFCApp.h : WinRTMFCApp アプリケーションのメイン ヘッダー ファイル
//
#pragma once

#ifndef __AFXWIN_H__
	#error "PCH に対してこのファイルをインクルードする前に 'stdafx.h' をインクルードしてください"
#endif

#include "resource.h"       // メイン シンボル


// CWinRTMFCAppApp:
// このクラスの実装については、WinRTMFCApp.cpp を参照してください
//

class CWinRTMFCAppApp : public CWinAppEx
{
public:
	CWinRTMFCAppApp() noexcept;


// オーバーライド
public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();

// 実装
	UINT  m_nAppLook;
	BOOL  m_bHiColorIcons;

	virtual void PreLoadState();
	virtual void LoadCustomState();
	virtual void SaveCustomState();

	afx_msg void OnAppAbout();
	DECLARE_MESSAGE_MAP()
};

extern CWinRTMFCAppApp theApp;
