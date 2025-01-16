<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WeightSheetLoads.aspx.cs" Inherits="WeightSheets_WeightSheetLoads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel runat="server" Id="UP1">
        <ContentTemplate>
            <h3><asp:Label runat="server" ID="lblHeader" Text=" WeightSheet Number"></asp:Label></h3>
            <br />
            <asp:GridView runat="server" ID="Grid1" HorizontalAlign="Center"  CssClass=" table table-hover " AutoGenerateColumns="False" DataKeyNames="UID" DataSourceID="SqlDataSource1" AllowSorting="True">

                <Columns>
                    <asp:TemplateField>
                        
                        <ItemTemplate>
                         <asp:LinkButton runat="server" ID="btnReprint" CssClass="btn btn-primary" Text="Reprint" PostBackUrl='<%# "~/WeightSheets/DailyReport.aspx?IntakeLoad=true&LoadUID="+ Eval("UID") %>' ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Load_Id" HeaderText="Load#" SortExpression="Load_Id"></asp:BoundField>
                    <asp:TemplateField HeaderText="Truck Id" SortExpression="Truck_Id">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Truck_Id") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Truck_Id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bol" SortExpression="Bol">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Bol") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Bol") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bin" SortExpression="Bin">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# Eval("Bin") %>'></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Bin") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Time_In" HeaderText="Time In" SortExpression="Time_In" DataFormatString="{0:t}"></asp:BoundField>
                    <asp:TemplateField HeaderText="Weight In" SortExpression="Weight_In">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Weight_In") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Weight_In", "{0:N0}") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Time_Out" HeaderText="Time Out" SortExpression="Time_Out" DataFormatString="{0:t}"></asp:BoundField>
                    <asp:TemplateField HeaderText="Weight Out" SortExpression="Weight_Out">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Weight_Out") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Weight_Out", "{0:N0}") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Comment") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("Comment") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Protein" SortExpression="Protein">
                        <EditItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Protein") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("Protein") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Net" HeaderText="Net" SortExpression="Net" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>

                  
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1"  CssClass="btn btn-danger" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                        </ItemTemplate>
                       
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton2" runat="server"  CssClass="btn btn-warning" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton3" runat="server"  CssClass="btn btn-default" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton2" runat="server"  CssClass="btn btn-default" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                        </ItemTemplate>
                       
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:NW_DataConnectionString %>' SelectCommand="SELECT Loads.UID, Loads.Load_Id, Loads.Location_Id, Loads.Server_Name, Loads.Sequence_Id, Loads.Time_In, Loads.Weight_In, Loads.Manual_Weight_In, Loads.Time_Out, Loads.Weight_Out, Loads.Manual_Weight_Out, Loads.Bol, Loads.Bin, Loads.Truck_Id, Loads.Comment, Loads.Void, CASE WHEN Inbound_Loads.UID IS NULL THEN Transfer_Loads.Protein ELSE Inbound_Loads.Protien END AS Protein, CASE WHEN Inbound_Loads.UID IS NULL THEN CONVERT (bit , 0) ELSE CONVERT (bit , 1) END AS Intake, ABS(ISNULL(Loads.Weight_Out, 0) - Loads.Weight_In) AS Net FROM Weight_Sheet_Transfer_Loads INNER JOIN Transfer_Loads ON Weight_Sheet_Transfer_Loads.UID = Transfer_Loads.Transfer_Load_UID RIGHT OUTER JOIN Loads LEFT OUTER JOIN Inbound_Loads ON Loads.UID = Inbound_Loads.Load_UID ON Transfer_Loads.Load_UID = Loads.UID WHERE (CASE WHEN Inbound_Loads.UID IS NULL THEN Weight_Sheet_Transfer_Loads.Weight_Sheet_UID ELSE Inbound_Loads.WS_UID END = @WSUID) ORDER BY Loads.Load_Id">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="WSUID" Name="WSUID"></asp:QueryStringParameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

