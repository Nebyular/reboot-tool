# reboot-tool
A tool to automate applications scheduled to run that may require immediate reboot on non-compliant devices. This tools is intended to give end users a more user friendly way to be notified of upcoming reboots for such a scenario, either to defer them temporarily or to commit immediately to rebooting.


### Configuring the tool

Edit the App.Config file and modify the following fields.

* &lt;add key="command" value="cmd.exe" /&gt;  &lt;!--Value to be set to the executable to run, can specify full qualified pathnames. i.e. cmd.exe or C:\Windows\System32\cmd.exe--&gt;

* &lt;add key="switches" value="ipconfig /all" /&gt;  &lt;!--Switches and additional arguments to be added to the executable. i.e. ipconfig or ipconfig /all--&gt;

* &lt;add key="initialTime" value="1800" /&gt;   &lt;!--The initial time for the timer in seconds i.e. 1800 (30 minutes)--&gt;

* &lt;add key="warningTime" value="300" /&gt;  &lt;!--The time threshold for when the window will reappear for the user to alert them of a remaining amount of time left. In seconds i.e. 300 (5 minutes)--&gt;

* &lt;add key="deferTime" value="3600" /&gt;  &lt;!--The time added each time the Defer button is selected. i.e. 3600 (1 hour)--&gt;
