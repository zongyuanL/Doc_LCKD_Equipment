#include <dlfcn.h>
#include <stdio.h>

void *pHandle;
int (*BANKCOMM_SetParam)(int iParamIndex, unsigned char * ucParamBuffer);
int (*BANKCOMM_Login)(char *out, char *deviceID);
int (*BANKCOMM_Purchase)(long amount, char* scancode, char* detail, char *out, char *deviceID);
int (*BANKCOMM_AutoPurchase)(long amount, char *scancode, char* detail, long timeout, char *out, char *deviceID);
int (*BANKCOMM_Query)(char* sysOrderNo, char *out, char *deviceID);
int (*BANKCOMM_Void)(long amount, char *sysOrderNo, char *out, char *deviceID);
int (*BANKCOMM_Refund)(long amount, char *sysOrderNo, char *out, char *deviceID);

void init()
{
    char *pErr;
    
    pHandle = dlopen("./libDLL_BANKCOMM_INTERFACE.so", RTLD_LAZY);	
    if(!pHandle)
	{
		printf("Failed load library\n");
    }
    pErr = dlerror();
	if(pErr != NULL)
	{
		printf("%s\n", pErr);
		return;
    }
    
    BANKCOMM_SetParam = dlsym(pHandle, "BANKCOMM_SetParam");	
    pErr = dlerror();
	if(pErr != NULL)
	{
		printf("%s\n", pErr);
		return;
	}

    BANKCOMM_Login = dlsym(pHandle, "BANKCOMM_Login");	
    pErr = dlerror();
	if(pErr != NULL)
	{
		printf("%s\n", pErr);
		return;
	}

	BANKCOMM_Purchase = dlsym(pHandle, "BANKCOMM_Purchase");
    pErr = dlerror();
	if(pErr != NULL)
	{
		printf("%s\n", pErr);
		return;
	}

	BANKCOMM_AutoPurchase = dlsym(pHandle, "BANKCOMM_AutoPurchase");
    pErr = dlerror();
	if(pErr != NULL)
	{
		printf("%s\n", pErr);
		return;
	}

	BANKCOMM_Query = dlsym(pHandle, "BANKCOMM_Query");
    pErr = dlerror();
	if(pErr != NULL)
	{
		printf("%s\n", pErr);
		return;
	}

	BANKCOMM_Void = dlsym(pHandle, "BANKCOMM_Void");
    pErr = dlerror();
	if(pErr != NULL)
	{
		printf("%s\n", pErr);
		return;
	}

	BANKCOMM_Refund = dlsym(pHandle, "BANKCOMM_Refund");
    pErr = dlerror();
	if(pErr != NULL)
	{
		printf("%s\n", pErr);
		return;
	}
}

void deInit()
{
    dlclose(pHandle);
}

typedef struct {
	unsigned char responseCode[2];
	unsigned char responseMsg[60];
	unsigned char amount[12];
	unsigned char txnDate[8];
	unsigned char txnTime[6];
	unsigned char orderID[32];
	unsigned char extendField[60];
} BANKCOMM_Result;

void dumpResult(BANKCOMM_Result *result)
{
	unsigned char buf[60+1];
	memset(buf, 0, sizeof(buf));
	memcpy(buf, result->responseCode, sizeof(result->responseCode));
	printf("responseCode = %s\n", buf);
	memset(buf, 0, sizeof(buf));
	memcpy(buf, result->responseMsg, sizeof(result->responseMsg));
	printf("responseMsg = %s\n", buf);
	memset(buf, 0, sizeof(buf));
	memcpy(buf, result->amount, sizeof(result->amount));
	printf("amount = %s\n", buf);
	memset(buf, 0, sizeof(buf));
	memcpy(buf, result->txnDate, sizeof(result->txnDate));
	printf("txnDate = %s\n", buf);
	memset(buf, 0, sizeof(buf));
	memcpy(buf, result->txnTime, sizeof(result->txnTime));
	printf("txnTime = %s\n", buf);
	memset(buf, 0, sizeof(buf));
	memcpy(buf, result->orderID, sizeof(result->orderID));
	printf("orderID = %s\n", buf);
	memset(buf, 0, sizeof(buf));
	memcpy(buf, result->extendField, sizeof(result->extendField));
	printf("extendField = %s\n", buf);
}

void trans()
{
    int iStatus;
    long amount = 0;
    char channel[2+32];
    char authcode[32];
    char sysorderno[32];
    int ret;
    BANKCOMM_Result result;
    
    while(1)
    {
		printf("Please Input TransType: \n[1]consume [2]query [3]void [4]refund [5]auto_consume  --- [8]init [9]sign in [0]quit\n");
		scanf("%d",&iStatus);
		switch(iStatus)
		{
			case 1:	//consume
				printf("Please Input Money:\n");
				scanf("%lu",&amount);
				printf("Please Input Channel:\n[01]wepay [02]alipay [03]cup\n");
				scanf("%s",channel);
				printf("Please Input AuthCode:\n");
				scanf("%s",channel+2);
				BANKCOMM_Purchase(amount, channel, "\xE4\xB8\xAD\xE6\x96\x87""detail-linux", (char*)&result, 0);
				dumpResult(&result);
				break;
			case 5:	//consume
				printf("Please Input Money:\n");
				scanf("%lu",&amount);
				printf("Please Input Channel:\n[01]wepay [02]alipay [03]cup\n");
				scanf("%s",channel);
				printf("Please Input AuthCode:\n");
				scanf("%s",channel+2);
				BANKCOMM_AutoPurchase(amount, channel, "\xE4\xB8\xAD\xE6\x96\x87""detail-linux", 45, (char*)&result, "06102");
				dumpResult(&result);
				break;
			case 2:
				printf("Please Input SysOrderNo:\n");
				scanf("%s",sysorderno);
				BANKCOMM_Query(sysorderno, (char*)&result, 0);
				dumpResult(&result);
				break;
			case 3:
				printf("Please Input Money:\n");
				scanf("%lu",&amount);
				printf("Please Input SysOrderNo:\n");
				scanf("%s",sysorderno);
				BANKCOMM_Void(amount, sysorderno, (char*)&result, 0);
				dumpResult(&result);
				break;
			case 4:
				printf("Please Input Money:\n");
				scanf("%lu",&amount);
				printf("Please Input SysOrderNo:\n");
				scanf("%s",sysorderno);
				BANKCOMM_Refund(amount, sysorderno, (char*)&result, 0);
				dumpResult(&result);
				break;
			case 8:
				BANKCOMM_SetParam(0, "01102");
				break;
			case 9:
				ret = BANKCOMM_Login((char*)&result, 0);
				dumpResult(&result);
				break;
			case 0:
				return;
			default:
				printf("error [transtype]  Please repeat input");
				break;
		}
    }
}

int main()
{
    init();
    trans();
    deInit();
}
