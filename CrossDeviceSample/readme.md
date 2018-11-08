# README

## ������

CrossDeviceSample �v���W�F�N�g�� CrossDeviceSample.UWP �v���W�F�N�g���f�o�b�O���s����B
Visual Studio �̏o�̓E�B���h�E�� Family Name �̃��O���o��̂ōT����B

### CrossDeviceSample.AppService �v���W�F�N�g�̕ҏW

`CommunicationService.cs` �� `ClientFamilyName` �̒l�� Desktop app's family name �̒l�ɂ���B

### CrossDeviceSample.WPF �v���W�F�N�g�̕ҏW

`Consts.cs` �� `CommunicationServiceHost` �̒l�� UWP app's family name �̒l�ɂ���B 



## Microsoft Graph explorer ���J��
https://developer.microsoft.com/ja-jp/graph/graph-explorer

PC �ɕR�Â��Ă���}�C�N���\�t�g�A�J�E���g�ŃT�C���C�����Ĉȉ��̃A�N�Z�X���Ƀ`�F�b�N�����čēx�T�C���C������B

```
Device.Read
Device.Command
```

### �f�o�C�X�� ID ���擾����

GET �� `https://graph.microsoft.com/beta/me/devices` ��@���ƃf�o�C�X�̃��X�g���o�Ă���̂� PC �� ID ���T���Ă����B

### �A�v���̋N��

POST �� `https://graph.microsoft.com/beta/me/devices/��قǎ擾�����f�o�C�X��ID/command` �ɑ΂��ėv���{���Ɉȉ��� JSON ���w�肵�ċN������ƃA�v�����N������B

```json
{
  "type": "LaunchUri",
  "payload": {
    "uri": "crossdevicesample:?message=LaunchFromGraphExplorer"
  }
}
```

### �R�}���h�̑��M

�A�v�����N��������v���{�����ȉ��̒l�ɕύX���邱�ƂŃA�v���̃e�L�X�g���X�V�ł���B

```json
{
  "type": "appService",
  "payload": { "message": "Update from graph explorer" },
  "packageFamilyName": "8954fcae-b3bb-4921-a00a-713ba593aa13_5ppbtxp1sbcde",
  "appServiceName": "CommunicationService"
}
```

## �d�g��

Microsoft Graph API �� Cross-device experience �Ƃ��� API ���g�����Ƃ� Microsoft Graph API(REST API) �o�R�œ����}�C�N���\�t�g�A�J�E���g�ɕR�Â��Ă���f�o�C�X�ɑ΂��đ��삪�ł���B
����A�A�v���̋N�����A�v���T�[�r�X�ɑ΂��ăR�}���h�𑗂邱�Ƃ��o���܂��B�����ł̓R�}���h�𑗂������ʂ͎󂯂Ă܂��񂪃|�X�g�o�b�N URL �Ȃǂ��w��ł���̂ŋ��炭�󂯎���͂��B

### �A�v���̋N���ɂ���

UWP �A�v���i�܂� Dekstop Bridge�j�̓v���g�R���ɑΉ��o���܂��B
�����I�ɂ̓v���g�R���ɑΉ����Ă���A�v���͑S�� Microsoft Graph API ����N�����邱�Ƃ��o���܂��B

����� WPF �A�v���� Desktop Bridge ���g���� UWP �����ăv���g�R���Ή��ɂ��ċN���ł���悤�ɂ��܂����B

### �R�}���h�̑��M�ɂ���

�R�}���h�ւ̑Ή��́ADesktop Bridge �A�v���P�i�ł͂Ȃ� App Service �������� UWP �A�v����ʂɍ쐬���Ă��܂��B
Microsoft Graph API ����̓����[�g�A�N�Z�X�������Ă��� App Service �ɑ΂��ăR�}���h�𑗂邱�Ƃ��o���܂��B����́AWPF �A�v���̋N������ UWP �A�v���� App Service �ɐڑ����ɂ����ĉ�����҂��Ă��܂��B

App Service �� Graph API �o�R�ŃR�}���h��������A�����ڑ����Ȃ����Ă��� WPF �A�v���ɑ΂��ē]�����Ă��܂��B
�]�����ꂽ���b�Z�[�W�����Ƃ� WPF �A�v���ł͉�ʂ̃e�L�X�g���X�V���Ă��܂��B

