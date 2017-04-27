<%@ Page Title="" Language="vb" AutoEventWireup="false" enableEventValidation="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="reports.aspx.vb" Inherits=".reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <%--<link rel="stylesheet" href="/resources/demos/style.css" />--%>
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            AutoComplete();
        });
        function AutoComplete() {
            $("#<%=txtSearch.ClientID %>").autocomplete({
                 autoFocus: true,
                 source: function (request, response) {
                     $.ajax({
                         url: "man_api.aspx/AutoComplete",
                         data: "{'SearchText': '" + request.term + "'}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {
                             response($.map(data.d, function (item) {
                                 return { 
                                     label: item.Rep_Name,
                                     val: item.Rep_ID
                                 }
                             }))
                         },
                         error: function (response) {
                             alert(response.responseText);
                         },
                         
                         failure: function (response) {
                             alert(response.responseText);
                         }
                     });
                 },
                 select: function (e, i) {
                     $('#<%= Rep_Value.ClientID %>').val(i.item.val);
                     __doPostBack("<%= btnGo.UniqueID%>", "OnClick");
                 },
                 minLength: 1,
             });
         }


        $(function () {
            $(".cal").datepicker({
                changeMonth: true,
                changeYear: true
            });
        });
    </script>

    <script>
        $(function () {
            var availableTags = ["URBAN OUTFITTERS/ ANTHROPOLOGIE~C100108", "TEST ACCOUNT~20000", "ONE KINGS LANE~C100084", "GLOBALTRANZ~C100052", "Abbyson Living LLC~C100949", "EVERFAST INC.~C100046", "CYMAX STORES~C100036", "KLAUSSNER FURNITURE INC...KHF~C100068", "COD CUSTOMERS~C100889", "APT2B~C100005", "SAMSON INTERNATIONAL~C100093", "KRAVET FURNITURE 2- ACTIVE~C100069", "MODLOFT~C100080", "ASPEN HOME~C100008", "Transit Systems Inc~C101002", "FAIRMONT DESIGNS~C100123", "FURNITURE EXPO~C100975", "FIVE STARS SOURCE~C100618", "HERITAGE HOME GROUP LLC C/O RICOH USA, INC.~C100850", "CREATIVE WALLCOVERINGS~C100035", "SHIP HAWK~C100667", "SPECIALTY DELIVERY SERVICES~C100864", "CANDELABRA~C100020", "SMALL MOVE SOLUTIONS~C100979", "APROPOS~C100004", "Chairish Inc~C101054", "CARTER TRANSPORTATION~C100022", "PAYROLL SHOPPING~C100087", "KATHY KUO DESIGNS~C100270", "ACKER BRYANT DESIGN~C100115", "A AMERICA~C100832", "DISTINCTIVE CHESTERFIELDS~C100040", "DURALEE FINE FURNITURE~C100041", "BRUNSCHWIG & FILS~C100018", "BAKER - 08 - MCGUIRE~C100011", "HOMESPUN DESIGN, INC.~C100239", "VAUGHAN BASSETT~C100111", "THE BELLA COTTAGE~C100831", "PLUSH INTERIORS~C100817", "KINWAI USA INC.~C100131", "NEWPORT COTTAGES-RIVERSIDE~C100136", "Homenature, Inc~C101043", "SOMERTON DWELLING~C101007", "EKLA HOME, INC~C100957", "Sweet Elle Handmade Furniture~C100385", "EMBASSY CARGO, INC~C100043", "URBAN HOME INC.~C100880", "WORLD BAZAAR OUTLET~C100144", "COMPANY C~C100031", "THEODORE ALEXANDER USA~C100104", "FLEMING & HOWLAND~C100050", "GROSSMAN FURNITURE~C100125", "RIKI FORTGANG INTERIORS DESIGN~C100596", "NATUZZI AMERICAS INC.~C100083", "COTTAGE CHIC~C100181", "MADERA HOME~C100076", "PREVIOUSLY OWNED BY A GAY MAN~C100342", "BAKER FURNITURE (MARKET) - HIGH POINT~C100866", "MAINE COTTAGE~C100311", "MONTAUK SOFA~C100988", "TOBY DREBIN DESIGN~C100105", "COTTAGE AND BUNGALOW~C100032", "Hedge House~C101017", "L & L DESIGNS~C100644", "NORRIS HOME FURNISHINGS~C100324", "Steve Silver Company~C101058", "DIANE DUROCHER-RAMSEY~C100200", "CHARLOTTE & IVY~C100027", "American Mini Mover~C101034", "PALECEK~C100086", "JAMIE STERN /TEXSTYLE~C100101", "JOHN ROSSELLI & ASSOCIATES~C100493", "ARASON ENTERPRISES, INC.~C100006", "RADIUS STYLE~C100089", "HIGH FASHION HOME~C100871", "ENGLISH FARMHOUSE FURNITURE~C100843", "KRISTIN DROHAN COLLECTION-ROSWELL~C100283", "SKLAR FURNISHINGS~C100373", "RENEWAL HOME DÉCOR, LLC~C100987", "MILLESIME~C100501", "BOLIER & CO~C100015", "Designs By Giorgio~C101061", "ELOQUENCE FURNITURE~C100168", "CLEAN DESIGN-CLAIRE PAQUIN~C100171", "PENINSULA HOME COLLECTION~C100335", "OUR AMERICAN HERITAGE~C100327", "STARK CARPET CORPORATION~C100793", "BRADLEY HOME FURNISHINGS INC~C100947", "REFINED FURNISHINGS~C100956", "LUXE HOME COMPANY~C100295", "WALTER E SMITHE~C100940", "ANTIQUES BY ZAAR~C100002", "PHILLIPS COLLECTION~C100861", "MAGGIE NIELSEN INTERIORS~C100309", "Woodgenius / Scenic Fixtures~C100991", "COTTAGE CHIC / NC~C100182", "LAYLA GRAYCE~C100071", "HOUNDSTOOTH, LLC~C100127", "FEA HOME INC.~C100870", "Boyds Specialty Sleep~C101051", "CALICO CORNER----BROOKFIELD~C100148", "SPROUT SAN FRANCISCO~C100674", "SASS INTERIORS~C100179", "MARCH LEGEND/ FABLE PORCH~C100394", "Metropolitan Warehouse & Delivery Comp.~C101018", "WINDOW DESIGN -WANDA WADE~C100935", "KIM SALMELA~C100276", "DS INTERIORS~C100210", "CENTURY FURNITURE~C100897", "DDC~C100038", "MSM PROPERTY DEVELOPMENT~C100958", "SCOTT JORDAN FURNITURE~C100666", "REVIVAL HOME~C100350", "ZINC DOOR, INC~C100114", "URBIA IMPORTS~C100109", "TCS DESIGNS INC~C100142", "Design MIX Furniture~C100982", "BERNIE & PHYL'S FURNITURE~C100844", "HUTTON HOME INC~C100057", "ITALY BY WEB~C100058", "EF LM~C100835", "ECOSELECT FURNITURE~C100149", "BLISS HOME & DESIGN-ECOMMERC~C100867", "AFA STORES~C100833", "CALICO CORNERS -OSPREY~C100508", "COACH BARN~C100120", "RIVERS SPENCER INTERIORS~C100937", "SCW INTERIORS~C100095", "Robyn Karp Design, Inc.~C100353", "ROCKER REFINED-CARLA JAMES~C100354", "MISSION AVENUE STUDIO~C100503", "ANTIQUE EXCHANGE~C100463", "Bed Bath & Beyond / c/o Berman Blake Associates~C101046", "ALICE PIRSU INTERIORS~C100932", "CREATIVE INTERIORS~C100034", "HOMEY DESIGN INC~C100240", "O' HENRY HOUSE LIMITED~C100513", "WILLEM SMITH & CO., LLC~C100442", "SCHWARTZ FURN - METUCHEN~C100362", "LEXINGTON HOME BRAND~C100073", "Farmhouse and Cottage~C101010", "GVE GLOBAL LLC~C100053", "COTTAGE CHIC-CHARLOTTE~C100899", "CURRAN CATALOG~C100203", "AVETEX FURNITURE~C100470", "Design Nouvelle-France~C100994", "CHRISTOPHER GUY~C100028", "CIRCA , LLC~C100603", "BUNNY WILLIAMS HOME~C100019", "HICKORY CHAIR~C100126", "FLORIDIAN FURNITURE COMPANY~C100193", "DULCEY CONNON HOME~C100212", "RED EGG RESOURCES~C100944", "WENDY EIGEN DESIGNS~C100438", "YVES DELORME, INC~C100810", "PRILEY LANE LLC~C100981", "SEMINOLE FURNITURE~C100096", "MJ Design Services~C101057", "MAYFLOWER HOUSE INTERIORS~C100950", "JUDITH NORMAN COLLECTION~C100266", "J LOGAN INTERIORS~C100059", "JEN GOING INTERIORS - WESTHAMPTON BEACH~C100255", "ATLANTIC FURNITURE OF BROWARD INC~C100931", "ARTISTIC FRAME~C100007", "2-B Modern~C100146", "BAKER FURNITURE  - HICKORY~C100853", "BREUNERS HOME FURNISHING~C100017", "CLIFF YOUNG LTD. (COD)~C100893", "CREATIVE WORKS - MILLBURN~C100725", "Interiors By M & S Inc~C101038", "LEXINGTON HOME BRANDS--NC~C100180", "EVERETT DESIGNS-MICHELE RIVERA~C100122", "ERIN GATES DESIGN LLC~C100989", "HOME HIGHLIGHTS + HIGHPOINTS~C100941", "GUEST~10000", "MARCIA CARUSELLE~C100763", "Mortise & Tenon~C101045", "SIMONS HOUSE LLC~C100702", "The Classy Cottage~C101044", "TRAMO - FLORIDA~C100106", "VALLEY FARM ANTIQUES~C100964", "VOGUE INTERIORS~C100805", "Southworth Interiors~C100978", "SHELLEY MCBEE INTERIORS~C100971", "ONE SOURCE PLUS INC~C100326", "NELLA VETRINA~C100321", "HENREDON INT DES SHM/DC~C100509", "FLEUR DESIGNS~C100922", "BEACH GLASS DESIGNS~C100477", "BAUM INTS.~C100475", "BABY BY DESIGN~C100564", "ANDREW J.VANETTEN~C100459", "CREATIVE WINDOW DESIGNS~C100605", "GOODMAN CHARLTON INC~C100915", "GLOBAL HOME~C100976", "Leather Creations, Inc.~C101040", "JODIE O'DESIGNS~C100873", "INTERIORS OF WASHINGTON~C100815", "MCWILLIAM - AUTORE INTERIORS~C100769", "MADISON MCCORD INTERIORS~C100306", "POLLY MOROZOV DESIGN STUDIO~C100339", "RANDOLPH ROSE COLLECTION INC~C100954", "RINA CARROLLI INTERIOR DESIGN LLC~C100590", "WALSH HILL DESIGN~C100591", "THARON ANDERSON DESIGN~C100395", "THE DESIGN TAP LLC-GINA GIARRUSSO~C100683", "SUSAN STRAUSS DESIGN~C100679", "SUITE NY LLC~C101009", "VANGUARD~C100110", "Rina Carrolli Interior Design, LLC~C101021", "RJB DESIGNS~C100830", "MAISON INTERIORS /MARY-ANNE HAYES~C100827", "MICHELLE LIND INT~C100658", "MGS DESIGN~C100430", "Michael Thomas Furniture, Inc.~C101035", "JOHN BARDSLEY INC~C100928", "JOHN LOECKE INC - HIGH POINT~C100130", "HICKORY CHAIR * PEARSON - HENREDON~C100054", "GDC HOME~C100967", "DUNN & TIGHE INTERIORS,INC.~C100614", "DB GROUP AMERICA LTD~C100037", "DONGHIA, INC.~C100824", "CISCO BROTHERS CORP~C100697", "CommerceHub (Furnishing 411)~C100872", "CHADDOCK HOME~C100821", "ANNABELLE'S FINE HOME~C100515", "ATTITUDE FURNISHINGS~C100010", "BEACHLEY FURNITURE~C100415", "BOFFI STUDIO MIAMI~C100014", "CHARLOTTE HILL INTERIORS~C100157", "CHARLES STEWART COMPANY~C100026", "COASTAL DELIVERY~C100514", "CKR DESIGNS INC~C100968", "CargoTrans~C101047", "CORSICAN FURNITURE INC~C100724", "ECLECTIC INTERIORS~C100220", "ELECTIC INTERIORS~C100160", "DOVETAIL DESIGN / ALISON SORRENTINO~C100456", "GAZE BURVILL LTD ~C100934", "FOUR HANDS IMPORTS~C100195", "J REDMOND DESIGNS~C100250", "HUFFMAN-KOOS FURNITURE~C100055", "NDR ITALIAN LIVING~C100320", "MODERN HISTORY FURNITURE~C100505", "MATTERS OF STYLE~C100917", "MATTHEWS & PARKER LTD.~C100410", "LAVENDER FIELDS~C100291", "LONNI PAUL INTERIORS DES~C100281", "RABBITT RABBITT~C100344", "PARLAY DESIGN / JARED DECKER~C100137", "PAINTED FURNITURE BARN~C100939", "SEDGWICK & BRATTLE-NY~C100365", "TRITTER FEEFER HOME COLLECTION~C100418", "SUSAN THORN INTERIORS INC~C100382", "SUTTERFIELD CONSIGNMENT GALLERY~C100600", "SWAIM DSGN UPH~C100549", "STUDIO 882, LLC~C100677", "THE FABRIC YARD~C100400", "THOMAS PUCKETT DESIGNS~C100412", "THE MENAGERIE LTD.~C100685", "Tuvalu Home~C101001", "ZELMAN STYLE INTERIORS~C100113", "SEED DESIGNS / STUDIO MIX~C100862", "ROSENBERRY ROOMS-EMILY BURNS~C100092", "SONJA STANIC~C100672", "SOUTH OF MARKET-ATLANTA~C100792", "STANLEY FURNITURE COMPANY~C100676", "STARK CARPET CORP~C100099", "SIMPLY HOME~C100790", "Miromar Design Center, Suite 190~C100502", "QUINTUS HOME~C100343", "LOUNGE FURNITURE~C100286", "LUCILLE LICHTENBERG INTS~C100075", "MARY LYNN O'SHEA~C100407", "MARIE SANNA DESIGNS, LLC~C101027", "INDESIGN INC~C100243", "KATE PERNOUD~C100874", "KIM E.COURTNEY INTERIORS & DESIGN~C100067", "LEATHERCRAFT, INC.~C100072", "LA DESIGN CONCEPTS~C100646", "GRACIOUS INTERIORS BY DIANA~C100226", "HOME WORKS~C100748", "DRAPARIES INTERIORS~C100205", "EGR DESIGNS~C100159", "EJ VICTOR~C100042", "E LAWERENCE DESIGNS LLC~C100215", "ELSA SOYARS~C100169", "CAROL FLANAGAN INTERIOR DESIGN~C100995", "C & T PAINT & WALLPAPER~C100147", "BEVERLY FURNITURE~C100834", "BEYOND BLUE INTERIORS~C100485", "BARTON SHARPE LIMITED ~C100248", "ANDREW MARTIN~C100001", "AB- Pak Mail US232~C101052"

            ];
            function split(val) {
                return val.split(/,\s*/);
            }
            function extractLast(term) {
                return split(term).pop();
            }

            $(".ClientList")
              // don't navigate away from the field on tab when selecting an item
              .on("keydown", function (event) {
                  if (event.keyCode === $.ui.keyCode.TAB &&
                      $(this).autocomplete("instance").menu.active) {
                      event.preventDefault();
                  }
              })
              .autocomplete({
                  minLength: 0,
                  source: function (request, response) {
                      // delegate back to autocomplete, but extract the last term
                      response($.ui.autocomplete.filter(
                        availableTags, extractLast(request.term)));
                  },
                  focus: function () {
                      // prevent value inserted on focus
                      return false;
                  },
                  select: function (event, ui) {
                      var terms = split(this.value);
                      // remove the current input
                      terms.pop();
                      // add the selected item
                      terms.push(ui.item.value);
                      // add placeholder to get the comma-and-space at the end
                      terms.push("");
                      this.value = terms.join(", ");
                      return false;
                  }
              });
        });

        $(function () {
            var availableTags1 = ["Alabama:AL",
"Alaska:AK",
"American Samoa:AS",
"Arizona:AZ",
"Arkansas:AR",
"California:CA",
"Colorado:CO",
"Connecticut:CT",
"Delaware:DE",
"Dist. of Columbia:DC",
"Florida:FL",
"Georgia:GA",
"Guam:GU",
"Hawaii:HI",
"Idaho:ID",
"Illinois:IL",
"Indiana:IN",
"Iowa:IA",
"Kansas:KS",
"Kentucky:KY",
"Louisiana:LA",
"Maine:ME",
"Maryland:MD",
"Marshall Islands:MH",
"Massachusetts:MA",
"Michigan:MI",
"Micronesia:FM",
"Minnesota:MN",
"Mississippi:MS",
"Missouri:MO",
"Montana:MT",
"Nebraska:NE",
"Nevada:NV",
"New Hampshire:NH",
"New Jersey:NJ",
"New Mexico:NM",
"New York:NY",
"North Carolina:NC",
"North Dakota:ND",
"Northern Marianas:MP",
"Ohio:OH",
"Oklahoma:OK",
"Oregon:OR",
"Palau:PW",
"Pennsylvania:PA",
"Puerto Rico:PR",
"Rhode Island:RI",
"South Carolina:SC",
"South Dakota:SD",
"Tennessee:TN",
"Texas:TX",
"Utah:UT",
"Vermont:VT",
"Virginia:VA",
"Virgin Islands:VI",
"Washington:WA",
"West Virginia:WV",
"Wisconsin:WI",
"Wyoming:WY"];
            function split(val) {
                return val.split(/,\s*/);
            }
            function extractLast(term) {
                return split(term).pop();
            }

            $(".StateList")
              // don't navigate away from the field on tab when selecting an item
              .on("keydown", function (event) {
                  if (event.keyCode === $.ui.keyCode.TAB &&
                      $(this).autocomplete("instance").menu.active) {
                      event.preventDefault();
                  }
              })
              .autocomplete({
                  minLength: 0,
                  source: function (request, response) {
                      // delegate back to autocomplete, but extract the last term
                      response($.ui.autocomplete.filter(
                        availableTags1, extractLast(request.term)));
                  },
                  focus: function () {
                      // prevent value inserted on focus
                      return false;
                  },
                  select: function (event, ui) {
                      var terms = split(this.value);
                      // remove the current input
                      terms.pop();
                      // add the selected item
                      terms.push(ui.item.value);
                      // add placeholder to get the comma-and-space at the end
                      terms.push("");
                      this.value = terms.join(", ");
                      return false;
                  }
              });
        });
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="top_cap">
        <asp:Literal ID="litCap" runat="server"></asp:Literal>
    </div>
      <%-- AUTO COMPLETE SEARCH--%>
        <div style="margin-left: auto; margin-right: auto; text-align: center">
            <b>Search</b>
            <asp:TextBox ID="txtSearch" placeholder="Report Search" runat="server" Width="500px" ></asp:TextBox>
            <asp:HiddenField ID="Rep_Value" runat="server" />
        </div>
        <br />
    <asp:Button ID="btnGo" runat="server" Text="Button" style="display:none;" />
   
    <div>
        <table>
            <tr>
                <td>
                    <asp:GridView ID="gvData" runat="server" CssClass="AppLabel" AutoGenerateColumns="False" DataKeyNames="RP_ID" Width="10px">
                        <RowStyle Wrap="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="S#">
                                <HeaderStyle HorizontalAlign="Center" Width="10px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <%# CType(Container, GridViewRow).RowIndex + 1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Enter Value" HeaderStyle-Width="10px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPara" runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="ddlPara" runat="server"></asp:DropDownList>
                                    <asp:FileUpload ID="FileUpload1" runat="server" Visible="false" />
                                    <br />
                                    <span style="font-style: italic; font-size: 7pt;">
                                        <asp:Literal ID="litofError" runat="server" Text='<%# Bind("Description")%>'> </asp:Literal></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:Button ID="btnXL" runat="server" Text="Get XL" />
        <asp:Button ID="btnView" runat="server" Text="View" />
    </div>
    <div style="margin-left: auto; margin-right: auto;">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server" CssClass="AppLabel"></asp:GridView>
                </td>
            </tr>
        </table>

    </div>
    <div style="color: #D8000C; background-color: #FFBABA; font-size: 14pt">
        <asp:Literal ID="litError" runat="server"></asp:Literal>

    </div>


</asp:Content>
