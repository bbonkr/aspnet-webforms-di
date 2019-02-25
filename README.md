# ASP.NET WebForms 프로젝트에서 DI 사용

ASP.NET 에서 의존성 주입을 사용하기 위해 Autofac 라이브러리를 사용해서 프로젝트를 구성합니다.

>   [Autofac: Application Integration > ASP.NET > Web Forms](https://autofaccn.readthedocs.io/en/latest/integration/webforms.html) 페이지에서 자세한 내용을 확인할 수 있습니다.



## ASP.NET Web Forms 프로젝트

ASP.NET Web Forms 프로젝트를 만듭니다. 이 때 대상 프레임워크는 .NET Framework 4.5 이상을 선택해야 합니다.

>   사용하려는 [Autofac 라이브러리](https://www.nuget.org/packages/Autofac/)가 .NET Framework 4.5 이상이 필요합니다.



## 의존 라이브러리 추가

[Nuget: Autofac.Web](https://www.nuget.org/packages/Autofac.Web/)을 추가합니다.



## 구성 파일 편집

프로젝트의 web.config 파일을 편집기에서 열고, 아래 내용을 추가합니다.

>   아래 구성은 멤버 속성 의존성 주입을 위한 구성입니다.

게시 대상이 **IIS6**인 경우

```xml
<system.web>
    <!-- 중략 -->
    <httpModules>
        <add name="ContainerDisposal"
             type="Autofac.Integration.Web.ContainerDisposalModule, Autofac.Integration.Web"/>
        <add name="PropertyInjection"
             type="Autofac.Integration.Web.Forms.PropertyInjectionModule, Autofac.Integration.Web"/>
    </httpModules>
    <!-- 중략 -->
</system.web>
```



게시 대상이 **IIS7+** 인 경우

```xml
<system.webServer>
    <modules>
        <!-- 중략 -->
        <add name="ContainerDisposal"
             type="Autofac.Integration.Web.ContainerDisposalModule, Autofac.Integration.Web"
             preCondition="managedHandler"/>
        <add name="PropertyInjection"
             type="Autofac.Integration.Web.Forms.PropertyInjectionModule, Autofac.Integration.Web"
             preCondition="managedHandler"/>
        <!-- 중략 -->
    </modules>
</system.webServer>
```



## 의존성 컨테이너 빌드 코드 추가

프로젝트의 Global.asax.cs 파일을 열고 아래 내용을 추가합니다.

```csharp
using Autofac;
using Autofac.Integration.Web;
// .. 중략 ...

namespace ProjectName 
{
    public class Global : HttpApplication, IContainerProviderAccessor
    {
        public IContainerProvider ContainerProvider { get => containerProvider; }

        void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();

            // 형식을 등록합니다.
            builder.RegisterType<SampleService>().As<ISampleService>();
            builder.RegisterType<SampleInnerService>().As<ISampleInnerService>();

            // ... 중략

			// 의존성 컨테이너를 초기화합니다.
            containerProvider = new ContainerProvider(builder.Build());
        }

        private static IContainerProvider containerProvider;
    }
}
```



## 예제

ISampleInnerService 인터페이스

```csharp
public interface ISampleInnerService
{
    string GetSomeString();
}
```



SampeInnerService 클래스

```csharp
public class SampleInnerService : ISampleInnerService
{
    public string GetSomeString()
    {
        return "Inner Service";
    }
}
```



ISampleService 인터페이스

```csharp
public interface ISampleService
{
    string GetString();
}
```



SampleService 클래스

```csharp
public class SampleService : ISampleService
{
    public SampleService(ISampleInnerService sampleInnerService)
    {
        this.sampleInnerService = sampleInnerService;
    }

    public string GetString()
    {
        return $"Hello, World! {sampleInnerService.GetSomeString()}";
    }

    ISampleInnerService sampleInnerService;
}
```

 

Default.aspx.cs 파일 

```csharp
public partial class _Default : PageBase
{
    public ISampleService sampleService;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = sampleService.GetString();
    }
}
```



웹 응용프로그램을 실행하면 Default.aspx 페이지가 열리고, 페이지의 제목이 `Hello, World! Inner Service - ..`로 출력됩니다.

IIS 가 HTTP 요청을 받으면 PropertyInjectionModule 이 실행될 페이지에 필요한 의존 형식의 인스턴스를 만들어서 주입한 후 실행됩니다.
